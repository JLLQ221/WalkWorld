using UnityEngine;

public class Paradax : MonoBehaviour
{
    [SerializeField] private Vector2 speedMove;

    private Vector2 offset;
    private Material material;
    private Rigidbody2D playerR;
    private BasicActor playerActor;

    private Vector2 lastPlayerPosition;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        if (playerActor != null)
        {
            playerActor = FindFirstObjectByType<Player>();
            playerR = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            lastPlayerPosition = playerR.position;
        }
    }

    private void Update()
    {
        if (playerActor != null)
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
        else
        {
            playerActor = FindFirstObjectByType<Player>();
            if (playerActor != null)
            {
                playerR = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
                lastPlayerPosition = playerR.position;
            }
        }
    }
}
