using UnityEngine;

public class FollowText : MonoBehaviour
{
    private Transform mainCam;
    private Transform unit;
    private Transform worldSpaceCanvas;
    private Vector3 offset;
    public bool followParent = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = UnityEngine.Camera.main.transform;
        unit = transform.parent;
        worldSpaceCanvas = GameObject.Find("WorldSpace").transform;

        transform.SetParent(worldSpaceCanvas);
    }

    // Update is called once per frame
    void Update()
    {
        if (!unit) { return; }
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position); // look at camera

        if (followParent)
        {
            transform.position = unit.position + offset;
        }
    }
}
