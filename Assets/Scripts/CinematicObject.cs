using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CinematicObject : MonoBehaviour
{
    public GameObject playerUI;
    public GameObject cinemticUI;

    [SerializeField] public BasicActor[] objects;
    private Entity[] actors;
    private bool active = false;

    [SerializeField] public List<CinematicAction> accions = new();

    private void Awake()
    {
        // actors tenga el mismo tamaño que objects
        actors = new Entity[objects.Length];

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                Entity entity = objects[i].GetEntity();
                if (entity != null)
                {
                    actors[i] = entity;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (!active)
        {
            if (player != null)
            {
                active = true;
                StartCoroutine(RunCinematic());
            }
        }
    }

    public void RunActions()
    {
        active = true;
        StartCoroutine(RunCinematic());
    }

    private IEnumerator RunCinematic()
    {
        playerUI.SetActive(false);
        cinemticUI.SetActive(true);

        foreach (var obj in objects)
        {
            obj.SetFreeMoving(true);
            obj.GetEntity().RunAction("stop");
        }

        for (int i = 0; i < accions.Count; i++)
        {
            yield return RunAccion(accions[i]);
        }


        foreach (var obj in accions)
        {
            obj.actor.SetFreeMoving(false);
        }

        foreach (var obj in objects)
        {
            obj.SetFreeMoving(false);
            obj.NormalMoving();
        }

        playerUI.SetActive(true);
        cinemticUI.SetActive(false);
    }


    IEnumerator RunAccion(CinematicAction accion)
    {
        var entity = accion.actor?.GetEntity();
        entity?.RunAction(accion.actionName);

        // Espera activa hasta que el actor indique que se puede continuar
        while (!accion.actor.GetContinue())
        {
            yield return null; // espera un frame antes de volver a checar
        }

        // Delay final antes de pasar a la siguiente acción
        if (accion.delay > 0f)
        {
            yield return new WaitForSeconds(accion.delay);
        }
    }
}


[System.Serializable]
public class CinematicAction
{
    public BasicActor actor;       // Referencia directa al actor
    public string actionName;    // Nombre de la acción a ejecutar
    public float delay;          // Tiempo de espera antes de la siguiente
}
