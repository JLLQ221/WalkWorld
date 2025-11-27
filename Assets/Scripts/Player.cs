using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BasicActor
{
    public Transform bodyUpper;
    public Transform bodyLower;
    public Transform groundCheck;

    private Animator animatorUpper;
    private Animator animatorLower;
    private Animator animatorBody;

    public float WalkSpeed;
    private float lastDash;
    private float life = 15;
    private float lastShoot;
    public float jumpForce = 4;
    private float groundRadios = 0.025f;
    private float damagueDirection;
    private int numerShoot = 0;
    public int maxDash;
    public int numberDash = 0;
    private int dashCount = 0;
    private bool dashRequest;
    public bool dashReloadOff = false;
    private bool damagueRequest;
    private bool[] arrayDash;
    private bool isGrounded;
    public bool moving = true;

    private InputAction m_moveAction;
    private InputAction m_shootAction;
    private InputAction m_dashAction;
    private InputAction m_upAction;
    private InputAction m_hover;
    private InputAction m_interaction;

    private Vector2 m_moveAmt;
    private Material materialOrigin;
    public Material materialWhite;
    public Input control;
    private SpriteRenderer spriteRenderer;
    public LayerMask groundLayer;
    public GameObject bulletPrefat;
    private Rigidbody2D m_rigidbody;
    private ObjectRecolect currentObject;
    private Coroutine dashActive;
    private Coroutine[] dashCorutines;

    protected override void Awake()
    {
        dashCorutines = new Coroutine[maxDash];
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_shootAction = InputSystem.actions.FindAction("Attack");
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_upAction = InputSystem.actions.FindAction("Jump");
        m_interaction = InputSystem.actions.FindAction("Interact");
        m_hover = InputSystem.actions.FindAction("Hover");
        arrayDash = new bool[maxDash];

        for (int i = 0; i < maxDash; i++)
        {
            arrayDash[i] = false;
        }

        m_rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        materialOrigin = spriteRenderer.material;
        animatorUpper = bodyUpper.GetComponent<Animator>();
        animatorLower = bodyLower.GetComponent<Animator>();
        animatorBody = GetComponent<Animator>();

        base.Awake();
        entity.UpdateAction("moveRigth", MoveRigth);
        entity.UpdateAction("moveLeft", MoveLeft);
        entity.UpdateAction("stop", Stop);
        entity.AddAction("dash", DashFree);
    }

    // Update is called once per frame
    void Update()
    {
        numberDash = maxDash - dashCount;
        if (freeMove) { return; }
        if (!moving)
        {
            m_moveAmt = new Vector2(0, 0);
            Walking();
            return;
        }

        // Se llama a la función ReadValue() para obtener un Vector2 con los valores del input del jugador.
        m_moveAmt = m_moveAction.ReadValue<Vector2>();

        // Modificamos la velocidad del jugador, de su objeto Rigidbody en su eje X y degamos intacto su eje Y.
        // Esto funciona ya que en su propiedad lienarVelocity le pasamos un Vector2, Vector 2D, que solo espera
        // eje X y Y 

        Walking();

        animatorUpper.SetBool("Shoot", m_shootAction.IsPressed());

        Vector3 direction;
        if (transform.localScale.x == 1.0f) direction = Vector2.right;
        else direction = Vector2.left;

        if (m_shootAction.IsPressed())
        {
            if (Time.time > lastShoot + 0.5f)
            {
                lastShoot = Time.time;
                Vector2 scale;
                if (numerShoot == 1)
                {
                    direction.y = 0.0f;
                    scale = new Vector2(0.5f * transform.localScale.x, 0.5f);
                    numerShoot = 0;
                }
                else
                {
                    direction.y = 0.2f;
                    scale = new Vector2(0.4f * transform.localScale.x, 0.4f);
                    numerShoot++;
                }
                GameObject bullet = Instantiate(bulletPrefat, transform.position + direction * 0.1f, Quaternion.identity);
                control.VibrationController(0.32f, 0.13f);
                bullet.GetComponent<BulletPlayer>().Scale(scale);
            }
        }
        else
        {
            numerShoot = 0;
        }

        if (m_dashAction.WasPerformedThisFrame() && dashCount < maxDash)
        {
            ShowOtherSprites(false);
            dashRequest = true;
        }

        if (m_upAction.WasPressedThisFrame() && isGrounded)
        {
            m_rigidbody.linearVelocity = new Vector2(m_rigidbody.linearVelocityX, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadios, groundLayer);
        animatorLower.SetFloat("VerticalVelocity", m_rigidbody.linearVelocityY);
        animatorUpper.SetFloat("VerticalVelocity", m_rigidbody.linearVelocityY);
        animatorLower.SetBool("IsGrounded", isGrounded);
        animatorUpper.SetBool("IsGrounded", isGrounded);

        if (dashRequest && dashCount < maxDash && !freeMove)
        {
            Dash();
            dashRequest = false;
        }

        if (damagueRequest)
        {
            int x = ((int)animatorBody.transform.localScale.x);
            float forceHit = 1f;
            m_rigidbody.linearVelocityX = Mathf.Sign(damagueDirection) * forceHit;
        }

        if (!dashReloadOff && dashCount > 0)
        {
            dashReloadOff = true;
            StartCoroutine(DashReloadCoroutine());
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadios);
        }
    }

    private void Walking()
    {
        //m_animator.SetFloat("Speed", Mathf.Abs(m_moveAmt.x)); // Usa el eje X para definir la velocidad de caminar
        bool isDirectionBack = ((int)Mathf.Sign(m_moveAmt.x * transform.localScale.x)) != 1;
        bool isHover = m_hover.IsPressed();

        float speed = (!isHover) ? m_moveAmt.x * WalkSpeed : (!isDirectionBack) ? m_moveAmt.x * WalkSpeed : m_moveAmt.x * 1.5f;
        float speedAnimation = (!m_hover.IsPressed()) ? 1.0f : (!isDirectionBack) ? 1f : 0.6f;
        animatorUpper.SetFloat("Speed", Mathf.Abs(speed));
        animatorLower.SetFloat("Speed", Mathf.Abs(speed));
        animatorUpper.SetFloat("SpeedAnimation", speedAnimation);
        animatorLower.SetFloat("SpeedAnimation", speedAnimation);

        // Le pasamos el eje X a la que se movera y el eje Y, linearVelocity ya ocupa el delta Time
        m_rigidbody.linearVelocity = new Vector2(speed, m_rigidbody.linearVelocity.y);

        if (speed != 0 && !isHover) animatorBody.transform.localScale = new Vector3(Mathf.Sign(speed), 1f, 1f);
    }

    private void MoveRigth()
    {
        float speed = 1.0f * 1.5f;
        m_rigidbody.linearVelocityX = speed;
        float valorScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(valorScale, transform.localScale.y, transform.localScale.z);
        animatorUpper.SetFloat("Speed", Mathf.Abs(speed));
        animatorLower.SetFloat("Speed", Mathf.Abs(speed));
    }

    private void MoveLeft()
    {
        float speed = 1.0f * -1.5f;
        m_rigidbody.linearVelocityX = speed;
        float valorScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(-valorScale, transform.localScale.y, transform.localScale.z);
        animatorUpper.SetFloat("Speed", Mathf.Abs(speed));
        animatorLower.SetFloat("Speed", Mathf.Abs(speed));
    }

    public void Stop()
    {

        foreach (var dash in dashCorutines)
        {
            if (dash != null)
            {
                StopCoroutine(dash);
            }
        }
        m_rigidbody.linearVelocity = Vector2.zero;

        for (int i = 0; i < dashCorutines.Length; i++)
        {
            dashCorutines[i] = null;
        }

        // Detener completamente el Rigidbody2D
        EnableCollision(false);
        ShowOtherSprites(true);

        dashRequest = false;
        if (dashActive != null)
        {
            StopCoroutine(dashActive);
        }
        animatorBody.SetBool("Dash", false);
        animatorUpper.SetBool("Shoot", false);
        animatorUpper.SetFloat("Speed", 0);
        animatorLower.SetFloat("Speed", 0);
    }

    public override void NormalMoving()
    {
        EnableCollision(false);
        ShowOtherSprites(true);
        dashRequest = false;
        // Detener completamente el Rigidbody2D
        m_rigidbody.linearVelocity = Vector2.zero;
        if (dashActive != null)
        {
            StopCoroutine(dashActive);
        }
        animatorBody.SetBool("Dash", false);
        animatorUpper.SetBool("Shoot", false);
        animatorUpper.SetFloat("Speed", 0);
        animatorLower.SetFloat("Speed", 0);
    }

    private void ShowOtherSprites(bool visible)
    {
        bodyUpper.GetComponent<SpriteRenderer>().enabled = visible;
        bodyLower.GetComponent<SpriteRenderer>().enabled = visible;
    }

    public void DashCoroutine()
    {
        int x = (m_moveAmt.x == 0) ? ((int)animatorBody.transform.localScale.x) : ((int)Mathf.Sign(m_moveAmt.x));
        dashCount++;
        float dashForce = 4.5f;
        m_rigidbody.AddForceX(x * dashForce, ForceMode2D.Impulse);
        dashCorutines[dashCount - 1] = StartCoroutine(DashMove(x, dashForce));
    }

    IEnumerator DashMove(int x, float dashForce)
    {
        for (int i = 0; i < 50; i++)
        {
            if (freeMove) { break; }
            m_rigidbody.AddForceX(x * dashForce, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.001f);
        }

        arrayDash[dashCount - 1] = true;
        // Restablece colisiones
        EnableCollision(false);

        animatorBody.SetBool("Dash", false);
        ShowOtherSprites(true);
        dashActive = null;
    }

    public void Dash()
    {
        animatorBody.SetBool("Dash", true);
        // Ignora colisiones entre Player y Enemy
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Attack"), true);
        EnableCollision(true);

        control.VibrationController(0.3f, 0.2f);
        DashCoroutine();
    }


    public void EnableCollision(bool enable)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), enable);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Attack"), enable);
    }

    public void DashFree()
    {
        ShowOtherSprites(false);
        animatorBody.SetBool("Dash", true);

        // Ignora colisiones entre Player y Enemy
        int x = (int)animatorBody.transform.localScale.x;
        float dashForce = 4.5f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Attack"), true);

        control.VibrationController(0.3f, 0.2f);
        m_rigidbody.AddForceX(x * dashForce, ForceMode2D.Impulse);
    }

    IEnumerator DashReloadCoroutine()
    {
        yield return new WaitForSeconds(1.2f);
        dashCount--;
        dashCorutines[dashCount] = null;
        arrayDash[dashCount] = false;
        dashReloadOff = false;
    }

    private void Dead()
    {
        life = 15;
        DamagueTeleport(0, -0.275f);
    }

    public void Damage(float direction)
    {

        if (life <= 0)
        {
            Dead();
        }
        else
        {
            damagueRequest = true;
            damagueDirection = direction;
            StartCoroutine(DamagueConturine());
            StartCoroutine(DamagueVisual());
            life--;
            control.VibrationController(0.5f, 0.2f);
        }
    }

    public void Damage()
    {
        if (life <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(DamagueVisual());
            life--;
            control.VibrationController(0.5f, 0.2f);
        }
    }

    public void DamagueTeleport(float x, float y)
    {
        Stop();
        StartCoroutine(MoveTeleport(x, y));
    }

    IEnumerator MoveTeleport(float x, float y)
    {
        DashFree();
        yield return new WaitForSeconds(0.1f);
        transform.position = new Vector3(x, y, 0);
        yield return new WaitForSeconds(0.1f);
        Stop();
    }

    IEnumerator DamagueConturine()
    {
        yield return new WaitForSeconds(0.25f);
        damagueRequest = false;
    }

    IEnumerator DamagueVisual()
    {
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.material = materialWhite;
            bodyLower.gameObject.GetComponent<SpriteRenderer>().material = materialWhite;
            bodyUpper.gameObject.GetComponent<SpriteRenderer>().material = materialWhite;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material = materialOrigin;
            bodyLower.gameObject.GetComponent<SpriteRenderer>().material = materialOrigin;
            bodyUpper.gameObject.GetComponent<SpriteRenderer>().material = materialOrigin;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public float getLife()
    {
        return life;
    }
}
