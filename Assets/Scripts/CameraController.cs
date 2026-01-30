using Unity.Cinemachine;
using UnityEngine;

public class CameraController : BasicActor
{
    public string cameraFind;
    public CinemachineCamera camera;
    public Transform[] objectsFollow;

    protected override void Awake()
    {
        base.Awake();

        entity.AddAction<float>("movePosition", MoveCameraPosition);
        entity.AddAction("cameraEnable", EnablePriority);
        entity.AddAction("activeObject", ActiveObject);
    }

    private void MoveCameraPosition(float xGo)
    {
        if (cameraFind.Length > 0)
        {
            camera = GameObject.Find(cameraFind).GetComponent<CinemachineCamera>();
        }

        if (camera != null)
        {
            camera.Priority = 10;
        }
    }

    private void EnablePriority()
    {
        camera.Priority = 0;
    }

    private void ActiveObject()
    {
        ActiveObject componetActive = camera.GetComponent<ActiveObject>();
        if (componetActive != null)
        {
            componetActive.ActiveObj();
        }
    }


    public override void NormalMoving()
    {
        Destroy(gameObject);
        if (camera != null)
        {
            camera.Priority = 0;
        }
    }
}
