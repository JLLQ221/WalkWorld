using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectStartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CinematicObject[] cinematicsStarts;
    public int cineaticInitial = 0;
    public float delayRunCinematic;
    public bool activeBlackPanel = false;
    void Start()
    {
        cinematicsStarts[0].StopPlayerActor();
        if (activeBlackPanel)
        {
            cinematicsStarts[0].ActivePanelBlack();
        }
        StartCoroutine(RunFirshCinematic());
    }

    private IEnumerator RunFirshCinematic()
    {
        float time = 0;
        while (time < delayRunCinematic)
        {
            time += Time.deltaTime;
            yield return null;
        }
        RunCinematic();
    }

    private void Update()
    {
        if (cinematicsStarts[cineaticInitial].IsDestroyed() && cineaticInitial < cinematicsStarts.Length)
        {
            cineaticInitial++;
            RunCinematic();
        }
    }

    private void RunCinematic()
    {
        cinematicsStarts[cineaticInitial].RunActions();
    }
}
