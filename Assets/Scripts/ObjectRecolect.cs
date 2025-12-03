using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectRecolect : ControllerText
{
    public GameObject playerUI;
    public SpriteRenderer spriteSecondary;
    public TextMeshProUGUI textKetInteract;

    private bool isColliderPlayer = false;
    private bool isInteractive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    new void Update()
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

        base.Update();

        if (isColliderPlayer && m_interaction.WasPressedThisFrame() && !dialogueStart)
        {
            if (!dialogueStart)
            {
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
            Destroy(gameObject);
        }
    }
}
