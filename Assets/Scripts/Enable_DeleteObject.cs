using System;
using UnityEngine;

public class Enable_DeleteObject : MonoBehaviour
{
    public Scene1Plants obj;
    public bool isDestroyObj = false;
    [SerializeField] public GameObject[] objectsGame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (obj.changeScene)
        {
            DisableObject();
            if (isDestroyObj)
            {
                DestroyObjects();
            }
        }
    }

    void DestroyObjects()
    {
        foreach (GameObject obj in objectsGame)
        {
            Destroy(obj.gameObject);
        }
    }

    void DisableObject()
    {
        foreach (GameObject obj in objectsGame)
        {
            obj.SetActive(false);
        }
    }
}
