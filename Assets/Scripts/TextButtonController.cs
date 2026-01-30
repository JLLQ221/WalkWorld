using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextButtonController : BasicActor
{
    public bool active = false;
    public SpritesButtonChema changeText;
    public TextButtonController showNext;
    private TextMeshProUGUI textButton;
    private CanvasGroup panel;
    [SerializeField, TextArea(3, 1)] public string globalText;
    [SerializeField, TextArea(3, 1)] public string pcText;
    [SerializeField, TextArea(3, 1)] public string controllerText;
    private string textChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void Awake()
    {
        textButton = GetComponentInChildren<TextMeshProUGUI>();
        panel = GetComponentInChildren<CanvasGroup>();
        if (textButton == null)
        {
            textButton = GetComponent<TextMeshProUGUI>();
        }
        base.Awake();
        entity.AddAction("active", ActiveText);
        entity.AddAction("desactive", Desactive);
        if (!active)
        {
            panel.DOFade(0f, 0f);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        bool usingGamepad = FindFirstObjectByType<InputDetector>().GetUsingGamepad();
        if (changeText != null && textButton != null && gameObject != null)
        {
            if (globalText.Length > 0)
            {
                textChange = globalText;
            }
            else
            {
                if (usingGamepad)
                {
                    textChange = controllerText;
                }
                else
                {
                    textChange = pcText;
                }
            }

            if (active)
            {
                textButton.text = changeText.ReplacePlaceholders(textChange, usingGamepad);
                textButton.spriteAsset = changeText.selectAsset;
            }
        }
    }

    public void ActiveText()
    {
        active = true;
        panel.DOFade(1f, 0.3f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void Desactive()
    {
        active = false;
        panel.DOFade(0f, 0.3f);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject != null)
            {
                panel.DOFade(0f, 0.45f).OnComplete(() =>
                {
                    if (showNext != null || !showNext.IsDestroyed())
                    {
                        showNext.ActiveText();
                    }
                    Destroy(gameObject);
                });
            }
        }
    }
}
