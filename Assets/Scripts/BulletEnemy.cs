using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private float direction;
    private float speed = 0;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        BulletPlayer bullet = collision.GetComponent<BulletPlayer>();
        bool objectTouch = collision.gameObject.layer == LayerMask.NameToLayer("Attack");
        Enemy enemy = collision.GetComponent<Enemy>();
        EnemyBasic enemyBasic = collision.GetComponent<EnemyBasic>();

        if (bullet != null || objectTouch || enemy != null || enemyBasic != null) return;
       
        if (player != null)
        {
            player.Damage(transform.localScale.x);
        }
        Destroy(gameObject);
    }
    public void Scale(Vector2 scale)
    {
        transform.localScale = scale;
        direction = Mathf.Sign(scale.x);
        speed = 2.2f;
    }
}
