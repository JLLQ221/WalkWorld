using UnityEngine;

public class CameraFollow : BasicActor
{
    internal static object main;
    public Transform target;
    private bool cameraFree = false;
    private float yOrigin = -0.075f;
    public float offsetY;
    public float speed = 5f; // La velocidad a la que se mueve la cámara
    public float suavizado = 0.01f; // El suavizado de la cámara
    private Vector3 velocity = Vector3.zero;

    protected override void Awake()
    {
        base.Awake();
        entity.AddAction("moveRigth", MoveRigth);
        entity.AddAction("moveLeft", MoveLeft);
        MovePlayer();
    }
    private void LateUpdate()
    {
        if (target == null || cameraFree) return;
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (target == null || cameraFree) return;
        Vector3 newPosition = new Vector3(
         target.position.x,
         target.position.y + offsetY,
         transform.position.z);

        // Opción 1: SmoothDamp (más estable)
        transform.position = Vector3.SmoothDamp(
            transform.position,
            newPosition,
            ref velocity,
            suavizado);
    }

    public void NotFollowPlayer()
    {
        cameraFree = true;
    }

    public void FollowPlayer()
    {
        cameraFree = false;
    }

    public void FollowObject(Transform target)
    {
        transform.position = new Vector3(target.position.x, yOrigin, transform.position.z);
    }

    public void MoveLeft()
    {
        float speed = -0.1f;
        MoveCamera(speed);
    }
    public void MoveRigth()
    {
        float speed = 0.1f;
        MoveCamera(speed);
    }

    public void MoveCamera(float speed)
    {
        transform.position = new Vector3(transform.position.x + speed, yOrigin, transform.position.z);
    }
}
