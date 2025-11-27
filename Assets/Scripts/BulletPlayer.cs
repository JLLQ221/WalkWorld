using UnityEngine;
using UnityEngine.UIElements;

public class BulletPlayer : MonoBehaviour
{
    private float direction;
    private float speed = 3.5f;
    private Rigidbody2D rb;
    public GameObject hitParticle;
    public ParticleSystem particuleEmisor;
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
        ParticuleEmisorDispatch();
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        BulletPlayer bullet = collision.GetComponent<BulletPlayer>();
        bool objectTouch = collision.gameObject.layer == LayerMask.NameToLayer("Attack");
        Enemy enemy = collision.GetComponent<Enemy>();
        if (player != null || bullet != null || objectTouch) return;
        CreateParticule();
        if (enemy != null)
        {
            enemy.Damague();
        }
        ParticuleEmisorDispatch();
        Destroy(gameObject);


    }
    private void ParticuleEmisorDispatch()
    {
        particuleEmisor.transform.parent = null;
        particuleEmisor.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        Destroy(particuleEmisor.gameObject, particuleEmisor.main.startLifetime.constantMax);
    }
    private void CreateParticule()
    {
        Vector3 position = new Vector3(transform.position.x + (0.04f * direction), transform.position.y, transform.position.z);
        GameObject ps = Instantiate(hitParticle, position, hitParticle.transform.rotation);
    }

    public void Scale(Vector2 scale)
    {
        transform.localScale = scale;
    }
}
