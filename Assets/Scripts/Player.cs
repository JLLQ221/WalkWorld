using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform bodyUpper;
    public Transform bodyLower;
    public Transform groundCheck;

    private Animator animatorUpper;
    private Animator animatorLower;
    private Animator animatorBody;

    private int numerShoot;
    public float WalkSpeed;
    private float lastDash;
    private float lastShoot;
    public float jumpForce = 4;
    private float groundRadios = 0.025f;
    private bool dashRequest;
    private bool isGrounded;
    public bool moving = true;

    private InputAction m_moveAction;
    private InputAction m_shootAction;
    private InputAction m_dashAction;
    private InputAction m_upAction;
    private InputAction m_interaction;

    private Vector2 m_moveAmt;
    public LayerMask groundLayer;
    public GameObject bulletPrefat;
    private Rigidbody2D m_rigidbody;
    private ObjectRecolect currentObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void Awake()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_shootAction = InputSystem.actions.FindAction("Attack");
        m_dashAction = InputSystem.actions.FindAction("Dash");
        m_upAction = InputSystem.actions.FindAction("Jump");
        m_interaction = InputSystem.actions.FindAction("Interact");
        m_rigidbody = GetComponent<Rigidbody2D>();

        animatorUpper = bodyUpper.GetComponent<Animator>();
        animatorLower = bodyLower.GetComponent<Animator>();
        animatorBody = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
                bullet.GetComponent<BulletPlayer>().Scale(scale);
            }
        }
        else
        {
            numerShoot = 0;
        }

        if (m_dashAction.WasPressedThisFrame() && Time.time > lastDash + 0.2f)
        {
            lastDash = Time.time;
            dashRequest = true;
            ShowOtherSprites(false);
            animatorBody.SetBool("Dash", true);
        }

        if (m_upAction.WasPressedThisFrame() && isGrounded)
        {
            m_rigidbody.linearVelocity = new Vector2(m_rigidbody.linearVelocityX, jumpForce);
        }

        animatorLower.SetFloat("VerticalVelocity", m_rigidbody.linearVelocityY);
        animatorUpper.SetFloat("VerticalVelocity", m_rigidbody.linearVelocityY);
        animatorLower.SetBool("IsGrounded", isGrounded);
        animatorUpper.SetBool("IsGrounded", isGrounded);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadios, groundLayer);
        if (dashRequest)
        {
            StartCoroutine(DashCoroutine());
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
        float speed = m_moveAmt.x * WalkSpeed;
        animatorUpper.SetFloat("Speed", Mathf.Abs(speed));
        animatorLower.SetFloat("Speed", Mathf.Abs(speed));

        // Le pasamos el eje X a la que se movera y el eje Y, linearVelocity ya ocupa el delta Time
        m_rigidbody.linearVelocity = new Vector2(speed, m_rigidbody.linearVelocity.y);

        if (speed != 0) animatorBody.transform.localScale = new Vector3(Mathf.Sign(speed), 1f, 1f);
    }

    private void ShowOtherSprites(bool visible)
    {
        bodyUpper.GetComponent<SpriteRenderer>().enabled = visible;
        bodyLower.GetComponent<SpriteRenderer>().enabled = visible;
    }

    public void Dash()
    {
        int x = (m_moveAmt.x == 0) ? ((int)animatorBody.transform.localScale.x) : ((int)Mathf.Sign(m_moveAmt.x));
        // float y = (m_moveAmt.y > 0) ? 0.05f : -0.05f;
        // Vector2 direction = new Vector2(x, y);

        //DasCount++;
        //if (DasCount == 1)
        //{
        //    Dash2.color = new Color(1f, 1f, 1f, 0f); // Blanco con alfa 0
        //}
        //else if (DasCount == 2)
        //{
        //    Dash1.color = new Color(1f, 1f, 1f, 0f); // Blanco con alfa 0
        //}

        float dashForce = 5f;
        m_rigidbody.AddForceX(x * dashForce, ForceMode2D.Impulse);
    }

    IEnumerator DashCoroutine()
    {
        // Ignora colisiones entre Player y Enemy
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        //VibrationController(0.3f, 0.2f);
        Dash();

        yield return new WaitForSeconds(0.25f); // Duración del Dash

        ShowOtherSprites(true);

        // Restablece colisiones
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);

        // 🔥 Quita la animación
        animatorBody.SetBool("Dash", false);

        dashRequest = false;
        //StartCoroutine(DashReload());
    }
}
