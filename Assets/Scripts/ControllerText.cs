using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerText : BasicActor
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueTextPanel;
    protected InputAction m_interaction;

    [SerializeField] public List<Dialogue> dialogos = new();

    private string actorNow = null;
    private string actorAfter = null;
    protected bool dialogueStart = false;
    private bool isContinueNormal = true;
    protected int lineIndex = 0;
    private int lineMax = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_interaction = InputSystem.actions.FindAction("Interact");
        actorNow = dialogos[0].actorTalkin;
    }

    protected override void Awake()
    {
        base.Awake();

        entity.AddAction<float>("maxLine", MaxLineShow);
        entity.AddAction("startDialogue", StartDialogue);
        entity.AddAction("continueNormal", ContinueNormalLine);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (dialogueStart && m_interaction.WasPressedThisFrame())
        {
            string dialogue = $"{(actorNow.Length == 0 ? actorAfter : actorNow)}: {dialogos[lineIndex].dialogueLine}";
            if (dialogueTextPanel.text.Equals(dialogue))
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
                dialogueTextPanel.text = dialogue;
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
            if (lineIndex < dialogos.Count)
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

    protected virtual void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogos.Count)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            dialogueStart = false;
            dialoguePanel.SetActive(false);
            continueStep = true;
            //Destroy(gameObject);
        }
    }

    private void SelectImgAndColor()
    {
        actorNow = dialogos[lineIndex].actorTalkin;
        dialogueTextPanel.text = $"{(actorNow.Length == 0 ? actorAfter : actorNow)}: ";
      
        if (actorNow.Equals(actorAfter) || actorNow.Length <= 0)
        {
            return;
        }

        switch (actorNow.ToLower())
        {
            case "anna":
                dialogueTextPanel.enableVertexGradient = false;
                dialogueTextPanel.color = Color.white;
                break;
            case "ia":
                // Dividor los numeros entre 255 para que quede en float
                dialogueTextPanel.colorGradientPreset = new TMP_ColorGradient(Color.white, new Color(0.8f, 0.99f, 1f), new Color(0.4f,0.78f,1f), new Color(0.24f,0.68f,0.98f));
                dialogueTextPanel.enableVertexGradient = true;
                break;
        }

        actorAfter = actorNow;
    }

    public void StartDialogue()
    {
        if (lineIndex < dialogos.Count)
        {
            dialogueStart = true;
            continueStep = false;
            dialoguePanel.SetActive(true);
            StartCoroutine(ShowLine());
        }
    }

    protected IEnumerator ShowLine()
    {
        dialogueTextPanel.text = string.Empty;
        SelectImgAndColor();

        foreach (char letter in dialogos[lineIndex].dialogueLine)
        {
            dialogueTextPanel.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public string actorTalkin = null;
    [SerializeField, TextArea(4, 6)] public string dialogueLine;
}
