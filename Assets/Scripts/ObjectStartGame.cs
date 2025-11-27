using UnityEngine;

public class ObjectStartGame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CinematicObject[] cinematicsStarts;
    public int cineaticInitial = 0;
    void Start()
    {
        StartObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartObject()
    {
        cinematicsStarts[cineaticInitial].RunActions();
    }
}
