using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CinematicObject : CameraController
{
    private GameObject playerUI;
    private GameObject cinemticUI;
    private GameObject screenBlack;
    private BasicActor player;
    private CinemachineBrain brainCamera;
    public bool offUIEnd = true;

    private bool active = false;
    public bool activeScreenBlack = false;
    [SerializeField] private bool chaseTransitionCamera = true;

    [SerializeField] public BasicActor[] actors;
    [SerializeField] public List<CinematicAction> accions = new();

    protected override void Awake()
    {
        base.Awake();
        // actors tenga el mismo tamaño que objects
        playerUI = GameObject.Find("CanvaUI").transform.Find("UIPlayer").gameObject;
        cinemticUI = GameObject.Find("CanvaUI").transform.Find("UICinematic").gameObject;
        screenBlack = GameObject.Find("CanvaUI").transform.Find("PanelBlack").gameObject;
        brainCamera = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();

        player = FindFirstObjectByType<Player>();

        foreach (var accion in accions)
        {
            if (accion.tag == ActorTag.Player)
            {
                accion.actor = player;
            }
        }
    }

    private void Start()
    {
        if (actors.Length <= 0)
        {
            actors = new BasicActor[1];
        }
        actors[0] = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!active)
        {
            if (collision.CompareTag("Player"))
            {
                active = true;
                StartCoroutine(RunCinematic());
            }
        }
    }

    public void StopActors()
    {
        foreach (var actor in actors)
        {
            actor.SetFreeMoving(true);
            actor.GetEntity().RunAction("stop");
        }
    }

    public void ActivePanelBlack()
    {
        screenBlack.SetActive(true);
    }

    public void StopPlayerActor()
    {
        actors[0].SetFreeMoving(true);
        actors[0].GetEntity().RunAction("stop");
    }

    public void RunActions()
    {
        active = true;
        StartCoroutine(RunCinematic());
    }

    private IEnumerator RunCinematic()
    {
        if (brainCamera != null && chaseTransitionCamera)
        {
            // Aquí está la forma correcta de asignar el blend
            brainCamera.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
        }
        else if (!chaseTransitionCamera)
        {
            brainCamera.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
            brainCamera.DefaultBlend.Time = 0;
        }

        playerUI.SetActive(false);
        cinemticUI.SetActive(true);

        StopActors();

        for (int i = 0; i < accions.Count; i++)
        {
            yield return RunAccion(accions[i]);
        }

        if (activeScreenBlack)
        {
            screenBlack.GetComponent<CanvasGroup>().DOFade(1f, 1.5f).SetEase(Ease.Linear);
            screenBlack.SetActive(true);
            screenBlack.transform.Find("Text").gameObject.SetActive(true);
        }

        if (!activeScreenBlack)
        {
            foreach (var obj in actors)
            {
                obj.SetFreeMoving(false);
                obj.NormalMoving();
            }


            if (offUIEnd)
            {
                playerUI.SetActive(true);
                cinemticUI.SetActive(false);
            }
        }

        if (chaseTransitionCamera)
        {
            StartCoroutine(ReturnCamera());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ReturnCamera()
    {
        float time = 0;
        float timeEnd = 1f;
        while (time < timeEnd)
        {
            yield return null;
        }
        if (brainCamera != null)
        {
            // Aquí está la forma correcta de asignar el blend
            brainCamera.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
            brainCamera.DefaultBlend.Time = 0;
        }
        Destroy(gameObject);
    }


    IEnumerator RunAccion(CinematicAction accion)
    {
        var entity = accion.actor?.GetEntity();
        entity?.RunAction(accion.actionName);

        // Espera activa hasta que el actor indique que se puede continuar
        while (!accion.actor.GetContinueStep())
        {
            yield return null; // espera un frame antes de volver a checar
        }

        // Delay final antes de pasar a la siguiente acción usando deltaTime
        if (accion.delay > 0f)
        {
            float elapsed = 0f;
            while (elapsed < accion.delay)
            {
                elapsed += Time.deltaTime;
                yield return null; // esperar un frame y acumular tiempo
            }
        }
    }
}


[System.Serializable]
public class CinematicAction
{
    public BasicActor actor;       // Referencia directa al actor
    public ActorTag tag;
    public string actionName;    // Nombre de la acción a ejecutar
    public float delay;          // Tiempo de espera antes de la siguiente
}

public enum ActorTag { Player, Enemy, Controller }
