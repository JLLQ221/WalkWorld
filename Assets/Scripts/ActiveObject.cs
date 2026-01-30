using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ActiveObject : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate;
    private CinemachineCamera cam;
    private GameManager gameManager;
    public PlayableDirector cinematic;
    public Scene1Plants plant;
    public CinematicObject cinematicObjects;
    private SoundManager soundManager;
    public bool activeObject = true;
    public bool activeSound = true;
    public int sceneIndex;

    private void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        cam = GetComponent<CinemachineCamera>();
        if (plant != null && activeObject)
        {
            if (plant.activeObject)
            {
                ActiveObj();
                if (plant.showCinematic && cinematic != null)
                {
                    PlayCinetic();
                }
            }
        }
    }

    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if (activeSound)
        {
            if (plant.changeScene && soundManager != null)
            {
                soundManager.PlaySoundAmbient(sceneIndex);
            }
        }
    }

    public void PlayCinetic()
    {
        StartCoroutine(InitialCinematic());
        plant.showCinematic = false;
    }

    IEnumerator InitialCinematic()
    {
        yield return new WaitForSeconds(0.1f);
        if (cinematicObjects != null)
        {
            cinematicObjects.RunActions();
        }
        cinematic.Play();
    }

    public void ActiveObj()
    {

        if (cam != null && cam.Priority != 10)
        {
            cam.Priority = 10;
        }

        if (objectsToActivate.Length > 0)
        {
            foreach (var obj in objectsToActivate)
            {
                obj.SetActive(true);
            }
        }

        if (cam != null && plant)
        {
            StartCoroutine(DisableCinematic());
        }
    }

    IEnumerator DisableCinematic()
    {
        float timeWait = 2.5f;
        float time = 0;
        while (time < timeWait)
        {
            time += Time.deltaTime;
            yield return null;
        }
        cam.Priority = 0;
        gameManager.ExitCinematic();
    }
}
