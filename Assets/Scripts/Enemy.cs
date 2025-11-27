using System.Collections;
using UnityEngine;
public class Enemy : BasicActor
{
    public Material materialWhite;
    private Material materialOrigin;
    public ObjectRecolect objectGuard = null;

    protected Rigidbody2D rgb2D;
    protected Animator animationEnemy;
    private Coroutine watchCorutina = null;

    protected int directionWatch = 0;
    public int life;
    public float stepsRight;
    public float stepsLeft;
    private bool wacthFree = false;
    protected float speed;
    private float xInitial;
    protected float scaleX;

    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        entity.AddAction("watch", WatchOnly);
        entity.UpdateAction("stop", Stop);
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialOrigin = spriteRenderer.material;
        scaleX = Mathf.Abs(transform.localScale.x);
    }

    protected virtual void Start()
    {
        rgb2D = GetComponent<Rigidbody2D>();
        animationEnemy = GetComponent<Animator>();
        xInitial = objectGuard == null ? transform.position.x : objectGuard.transform.position.x;
        if (objectGuard != null) objectGuard.EnableInteractive(false);
        stepsRight = xInitial + stepsRight;
        stepsLeft = xInitial - stepsLeft;
    }

    public void Dead()
    {
        if (objectGuard != null) objectGuard.EnableInteractive(true);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected void Watch()
    {
        float x = transform.position.x;
        switch (directionWatch)
        {
            case 0:
                if (x < stepsRight)
                {
                    speed = 1.0f * 1f;
                }
                else
                {
                    directionWatch = 1;
                }
                break;
            case 1:
                if (x > stepsLeft)
                {
                    speed = 1.0f * -1f;
                }
                else
                {
                    directionWatch = 0;
                }
                break;
            case 2:
                if (x >= xInitial - 0.2 && x <= xInitial + 0.5)
                {
                    directionWatch = 0;
                }
                else
                {
                    float diferential = Mathf.Abs(xInitial) - x;
                    speed = 1.5f * Mathf.Sign(diferential);
                }
                break;
        }

        transform.localScale = new Vector3(scaleX * Mathf.Sign(speed), transform.localScale.y, transform.localScale.z);
    }

    private void WatchOnly()
    {
        wacthFree = true;
        watchCorutina = StartCoroutine(WatchCorutine());
    }

    private void Stop()
    {
        if (watchCorutina != null)
        {
            StopCoroutine(watchCorutina);
            watchCorutina = null;
        }
        wacthFree = false;
        rgb2D.linearVelocity = Vector2.zero;
        animationEnemy.SetFloat("Speed", 0f);
    }

    public override void NormalMoving()
    {
        if (watchCorutina != null)
        {
            StopCoroutine(watchCorutina);
            watchCorutina = null;
        }
        wacthFree = false;
        rgb2D.linearVelocity = Vector2.zero;
        animationEnemy.SetFloat("Speed", 0f);
    }
    IEnumerator WatchCorutine()
    {
        while (wacthFree) // mantiene la corutina viva
        {
            Watch();
            rgb2D.linearVelocityX = speed;
            animationEnemy.SetFloat("Speed", Mathf.Abs(speed));
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Damague()
    {
        life--;
        if (life <= 0)
        {
            Dead();
            return;
        }
        StartCoroutine(HitGlow());
    }
    IEnumerator HitGlow()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.material = materialWhite;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material = materialOrigin;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
