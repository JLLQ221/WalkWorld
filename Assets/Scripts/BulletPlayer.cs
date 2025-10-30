using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    private float direction;
    private float speed = 2.2f;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = FindFirstObjectByType<Player>().transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;
        rb.linearVelocityX = direction * speed;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Scale(Vector2 scale)
    {
        transform.localScale = scale;
    }
}
