using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRecolect : MonoBehaviour
{
    [SerializeField, TextArea(4, 6)] public string[] dialogueLines;

    public GameObject dialoguePanel;
    public GameObject playerUI;
    public SpriteRenderer spriteSecondary;
    public TextMeshProUGUI textKetInteract;
    public TextMeshProUGUI dialogueTextPanel;
    private InputAction m_interaction;

    private bool isColliderPlayer = false;
    private bool didDialogueStart;
    private bool isInteractive = true;
    private int lineIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        m_interaction = InputSystem.actions.FindAction("Interact");
    }
    void Update()
    {
        bool usingGamepad = FindFirstObjectByType<Input>().usingGamepad;

        if (isColliderPlayer && isInteractive)
        {
            if (usingGamepad)
            {
                SetTextInteractive("Y");
            }
            else if (!usingGamepad)
            {
                SetTextInteractive("E");
            }
        }


        if (didDialogueStart && m_interaction.WasPressedThisFrame())
        {
            if (dialogueTextPanel.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueTextPanel.text = dialogueLines[lineIndex];
            }
        }

        if (isColliderPlayer && m_interaction.WasPressedThisFrame() && !didDialogueStart)
        {
            if (!didDialogueStart)
            {
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

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            Destroy(gameObject);
            Player player = FindFirstObjectByType<Player>();
            player.moving = true;
            playerUI.SetActive(true);
        }
    }

    public void StartDialogue()
    {
        playerUI.SetActive(false);
        textKetInteract.enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        spriteSecondary.enabled = false;
        Player player = FindFirstObjectByType<Player>();
        player.moving = false;
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }

    public void EnableInteractive(bool enable )
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

    private IEnumerator ShowLine()
    {
        dialogueTextPanel.text = string.Empty;

        foreach (char letter in dialogueLines[lineIndex])
        {
            dialogueTextPanel.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
