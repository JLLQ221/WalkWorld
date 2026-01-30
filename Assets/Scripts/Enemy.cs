using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : BasicActor
{
    public Material materialWhite;
    private Material materialOrigin;
    public ObjectRecolect objectGuard = null;
    public TextPoints prefabPoints;
    public Player player;
    public EnemyInfo enemyInfo;
    protected bool attack = false;

    protected AudioSource audio;
    protected Rigidbody2D rgb2D;
    protected Animator animationEnemy;
    protected float lastStep;
    private Coroutine watchCorutina = null;
    public ExplosionObject explosionPrefab;
    public bool explosionFirst = false;
    private bool explosionPast = false;

    protected int directionWatch = 0;
    public int life;
    public float stepsRight;
    public int pointsHave;
    public float stepsLeft;
    private bool wacthFree = false;
    protected float speed;
    private float xInitial;
    public bool dead = false;
    protected float scaleX;

    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        entity.AddAction("watch", WatchOnly);
        entity.UpdateAction("stop", Stop);
        entity.UpdateAction("moveLeft", MoveLeft);
        entity.UpdateAction("moveRight", MoveRigth);
        entity.UpdateAction("attack", Attack);
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialOrigin = spriteRenderer.material;
        scaleX = Mathf.Abs(transform.localScale.x);

        life = enemyInfo.life;
        speed = enemyInfo.speed;
    }

    protected virtual void Start()
    {
        player = FindFirstObjectByType<Player>();
        audio = GetComponent<AudioSource>();
        rgb2D = GetComponent<Rigidbody2D>();
        animationEnemy = GetComponent<Animator>();
        xInitial = objectGuard == null ? transform.position.x : objectGuard.transform.position.x;
        if (objectGuard != null) objectGuard.EnableInteractive(false);
        stepsRight = xInitial + stepsRight;
        stepsLeft = xInitial - stepsLeft;
        if (dead) Dead();
    }

    public void CreateText()
    {
        GameObject canvasUI = GameObject.Find("CanvaUI");
        GameObject uiPlayer = canvasUI.transform.Find("UIPlayer").gameObject;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasUI.transform as RectTransform,
            screenPos,
            null, // Overlay
            out uiPos
        );

        TextPoints instance = Instantiate(prefabPoints, uiPlayer.transform);
        instance.transform.SetAsFirstSibling(); // asegura que quede atrás
        instance.SetText(pointsHave);
        instance.GetComponent<RectTransform>().anchoredPosition = uiPos;
    }

    public void Attack()
    {
        attack = true;
        animationEnemy.SetBool("Attack", attack);
        audio.PlayOneShot(enemyInfo.GetSound(EnemySoundType.Attack));
    }

    public void Dead()
    {
        CreateText();
        if (objectGuard != null) objectGuard.EnableInteractive(true);
        freeMove = true;
        Stop();
        rgb2D.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<Collider2D>().enabled = false;
        AnimatoDead();
    }

    public void AnimatoDead()
    {
        if (explosionFirst)
        {
            CreateExplosions();
            explosionPast = true;
        }
        // Animación de fade con DOTween
        animationEnemy.SetBool("Dead", true);
        // Cuando termine el fade, instanciamos explosiones
    }

    public void CreateExplosions()
    {
        if (!explosionPast)
        {
            explosionPast = true;
            spriteRenderer.DOFade(0.5f, 0);
            // Posición exactamente encima del enemigo
            Vector3 spawnPos = transform.position;

            // Instancia una explosión justo encima
            Instantiate(explosionPrefab, spawnPos, Quaternion.identity);

            // Varias explosiones muy cerca del enemigo
            for (int i = 0; i < 3; i++)
            {
                Vector3 offset = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
                Instantiate(explosionPrefab, spawnPos + offset, Quaternion.identity);
            }
            audio.PlayOneShot(enemyInfo.GetSound(EnemySoundType.Death));
        }

        if (!explosionFirst)
        {
            Destroy(gameObject, 1);
        }
        explosionFirst = false;
    }


    private void MoveRigth()
    {
        float speed = 1.0f * 1.5f;
        rgb2D.linearVelocityX = speed;
        float valorScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(valorScale, transform.localScale.y, transform.localScale.z);
        animationEnemy.SetFloat("Speed", Mathf.Abs(speed));
        PlaySoundStep(speed);
    }

    private void MoveLeft()
    {
        float speed = 1.0f * -1.5f;
        rgb2D.linearVelocityX = speed;
        float valorScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(-valorScale, transform.localScale.y, transform.localScale.z);
        animationEnemy.SetFloat("Speed", Mathf.Abs(speed));
        PlaySoundStep(speed);
    }

    private void PlaySoundStep(float speed)
    {
        if (speed != 0 && Time.time > lastStep + 0.45f)
        {
            lastStep = Time.time;
            audio.PlayOneShot(enemyInfo.GetSound(EnemySoundType.Step));
        }
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

        PlaySoundStep(speed);

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
            float time = 0;
            float timeEnd = 0.01f;
            while (time < timeEnd)
            {
                time += Time.deltaTime;
                yield return null;
            }
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
            float time = 0;
            float timeEnd = 0.1f;
            while (time < timeEnd)
            {
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
            spriteRenderer.material = materialOrigin;
            while (time < timeEnd)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
