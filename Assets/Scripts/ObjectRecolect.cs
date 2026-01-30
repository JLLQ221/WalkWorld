using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRecolect : ControllerText
{
    public GameObject playerUI;
    public SpriteRenderer spriteSecondary;
    private SpriteRenderer spritePrimary;
    public TextMeshProUGUI textKetInteract;
    public SpritesButtonChema textChange;

    private bool isColliderPlayer = false;
    private bool isInteractive = true;


    private void Awake()
    {
        base.Awake();
        playerUI = GameObject.Find("UIPlayer");
        textKetInteract = GetComponentInChildren<TextMeshProUGUI>();
        spriteSecondary = GetComponentsInChildren<SpriteRenderer>()[1];
        spritePrimary = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    new void Update()
    {
        bool usingGamepad = FindFirstObjectByType<InputDetector>().GetUsingGamepad();

        if (isColliderPlayer && isInteractive)
        {
            textKetInteract.text = textChange.ReplacePlaceholders("{INTERACT}", usingGamepad);
            textKetInteract.spriteAsset = textChange.selectAsset;
        }
        else
        {
            SetTextInteractive("");
        }

        base.Update();

        if (isColliderPlayer && m_interaction.WasPressedThisFrame() && !dialogueStart)
        {
            if (!dialogueStart)
            {
                isInteractive = false;
                spriteSecondary.enabled = false;
                spritePrimary.enabled = false;
                Player player = FindFirstObjectByType<Player>();
                player.moving = false;
                player.Stop();
                StartDialogue();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (gameObject && player != null)
        {
            isColliderPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (gameObject && player != null)
        {
            isColliderPlayer = false;
            SetTextInteractive("");
        }
    }

    public void EnableInteractive(bool enable)
    {
        isInteractive = enable;
    }

    public void SetTextInteractive(string text)
    {
        if (textKetInteract != null)
        {
            textKetInteract.text = text;
        }
    }

    protected override void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogos.Count)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            dialoguePanel.SetActive(false);
            continueStep = true;
            Player player = FindFirstObjectByType<Player>();
            player.moving = true;
            player.AddSedd(1);
            Destroy(gameObject);
        }
    }
}
