using UnityEngine;

public class Camera : MonoBehaviour
{
    internal static object main;
    public Transform target;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y + 0.3f, transform.position.z);
    }
}
