using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CinemachineCamera camera = GetComponent<CinemachineCamera>();
        camera.Follow = FindFirstObjectByType<Player>().transform;
    }
}
