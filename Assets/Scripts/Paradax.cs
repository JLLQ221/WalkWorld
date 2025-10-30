using UnityEngine;

public class Paradax : MonoBehaviour
{
    [SerializeField] private Vector2 speedMove;

    private Vector2 offset;

    private Material material;

    private Rigidbody2D player;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        offset = (player.linearVelocityX * 0.1f) * speedMove * Time.deltaTime;
        material.mainTextureOffset += offset; 
    }
}
