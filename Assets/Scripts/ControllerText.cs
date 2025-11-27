using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerText : BasicActor
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueTextPanel;
    private InputAction m_interaction;

    [SerializeField, TextArea(4, 6)] public string[] dialogueLines;

    private bool dialogueStart = false;
    private bool isContinueNormal = true;
    private int lineIndex = 0;
    private int lineMax = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_interaction = InputSystem.actions.FindAction("Interact");
    }

    protected override void Awake()
    {
        base.Awake();

        entity.AddAction<float>("maxLine", MaxLineShow);
        entity.AddAction("startDialogue", StartDialogue);
        entity.AddAction("continueNormal", ContinueNormalLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueStart && m_interaction.WasPressedThisFrame())
        {
            if (dialogueTextPanel.text == dialogueLines[lineIndex])
            {
                if (isContinueNormal)
                {
                    NextDialogueLine();
                }
                else
                {
                    NextLineFor();
                }
            }
            else
            {
                StopAllCoroutines();
                dialogueTextPanel.text = dialogueLines[lineIndex];
            }
        }
    }

    private void MaxLineShow(float stopLine)
    {
        isContinueNormal = false;
        lineMax = lineIndex + ((int)stopLine - 1);
    }

    private void ContinueNormalLine()
    {
        isContinueNormal = true;
    }

    private void NextLineFor()
    {
        if (lineIndex < lineMax)
        {
            lineIndex++;
            if (lineIndex < dialogueLines.Length)
            {
                StartCoroutine(ShowLine());
            }
            else
            {
                dialogueStart = false;
                dialoguePanel.SetActive(false);
                //Destroy(gameObject);
                continueStep = true;
            }
        }
        else
        {
            lineIndex++;
            dialogueStart = false;
            dialoguePanel.SetActive(false);
            //Destroy(gameObject);
            continueStep = true;
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
            dialogueStart = false;
            dialoguePanel.SetActive(false);
            //Destroy(gameObject);
            continueStep = true;
        }
    }

    public void StartDialogue()
    {
        dialogueStart = true;
        dialoguePanel.SetActive(true);
        continueStep = false;
        StartCoroutine(ShowLine());
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
