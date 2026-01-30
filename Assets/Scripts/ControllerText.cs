using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerText : BasicActor
{
    protected GameObject dialoguePanel;
    protected TextMeshProUGUI dialogueTextPanel;
    protected TextMeshProUGUI textActorTalk;
    protected InputAction m_interaction;
    protected GameObject canvaUI;
    protected GameObject panelTalkActor;
    protected Animator imgActorTalk;

    [SerializeField] public List<Dialogue> dialogos = new();

    private Actor actorNow;
    private Actor actorAfter = Actor.None;
    protected bool dialogueStart = false;
    private bool isContinueNormal = true;
    protected int lineIndex = 0;
    private int lineMax = -1;

    protected override void Awake()
    {
        canvaUI = GameObject.Find("CanvaUI");
        // Encuentra el hijo aunque esté desactivado
        dialoguePanel = canvaUI.transform.Find("DialoguePanel").gameObject;
        panelTalkActor = dialoguePanel.transform.Find("PanelTalkActor").gameObject;
        imgActorTalk = dialoguePanel.transform.Find("PanelImgTalk").GetComponentInChildren<Animator>();
        textActorTalk = panelTalkActor.GetComponentInChildren<TextMeshProUGUI>(true);
        dialogueTextPanel = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>(true);
        base.Awake();

        entity.AddAction<float>("maxLine", MaxLineShow);
        entity.AddAction("startDialogue", StartDialogue);
        entity.AddAction("continueNormal", ContinueNormalLine);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_interaction = InputSystem.actions.FindAction("Interact");
    }


    // Update is called once per frame
    protected void Update()
    {
        if (dialogueStart && m_interaction.WasPressedThisFrame())
        {
            string dialogue = dialogos[lineIndex].dialogueLine;
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

    public override void NormalMoving()
    {
        Destroy(gameObject);
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
        actorNow = dialogos[lineIndex].actor;

        if (actorNow.Equals(actorAfter))
        {
            return;
        }

        imgActorTalk.SetInteger("ActorTalk", ((int)actorNow));
        textActorTalk.text = actorNow.ToString();

        switch (actorNow)
        {
            case Actor.Anna:
                dialogueTextPanel.enableVertexGradient = false;
                textActorTalk.enableVertexGradient = false;
                dialogueTextPanel.color = Color.white;
                textActorTalk.color = Color.white;
                break;
            case Actor.IA:
                // Dividor los numeros entre 255 para que quede en float
                TMP_ColorGradient colorIA = new(Color.white, new Color(0.8f, 0.99f, 1f), new Color(0.4f, 0.78f, 1f), new Color(0.24f, 0.68f, 0.98f));
                dialogueTextPanel.colorGradientPreset = colorIA;
                textActorTalk.colorGradientPreset = colorIA;
                textActorTalk.enableVertexGradient = true;
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
        float delay = dialogos[lineIndex].delay == 0 ? 0.05f : dialogos[lineIndex].delay;

        string line = dialogos[lineIndex].dialogueLine;
        for (int i = 0; i < line.Length; i++)
        {
            // Si detectamos inicio de etiqueta
            if (line[i] == '<')
            {
                int closingIndex = line.IndexOf('>', i);
                if (closingIndex != -1)
                {
                    // Añadimos la etiqueta completa sin delay
                    string tag = line.Substring(i, closingIndex - i + 1);
                    dialogueTextPanel.text += tag;
                    i = closingIndex; // saltamos hasta el final de la etiqueta
                    continue;
                }
            }

            // Añadimos letra normal con delay
            dialogueTextPanel.text += line[i];
            float time = 0;
            while (time < delay)
            {
                time += Time.deltaTime;
                yield return null; // esperar un frame y acumular tiempo
            }
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public Actor actor;
    [SerializeField, TextArea(4, 6)] public string dialogueLine;
    public float delay = 0.05f;
}

public enum Actor { Anna, IA, None }