using UnityEngine;

public class Paradax : MonoBehaviour
{
    [SerializeField] private Vector2 speedMove;

    private Vector2 offset;
    private Material material;
    private Rigidbody2D playerR;
    private BasicActor playerActor;

    private Vector2 lastPlayerPosition;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        playerActor = GameObject.FindGameObjectWithTag("Player").GetComponent<BasicActor>();
        playerR = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        lastPlayerPosition = playerR.position;
    }

    private void Update()
    {
        Vector2 currentPosition = playerR.position;
        Vector2 deltaPosition = currentPosition - lastPlayerPosition;

        if (!playerActor.GetFreeMovie())
        {
            offset = (deltaPosition * 11.8f) * speedMove * Time.deltaTime;
            material.mainTextureOffset += offset;
        }

        lastPlayerPosition = currentPosition;
    }
}
