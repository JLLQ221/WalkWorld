using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    private InputDetector input;
    protected InputAction m_interaction;
    public SpritesButtonChema textChange;
    private TextMeshProUGUI text;
    private GameObject repositionObject;
    private GameManager gameManager;
    public Vector2 positionMove;
    private bool showText = false;
    public int sceneChange;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        input = FindFirstObjectByType<InputDetector>();
        gameManager = FindAnyObjectByType<GameManager>();
        m_interaction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        ShowText();
        if (m_interaction.WasPressedThisFrame() && showText)
        {
            showText = false;
            text.text = "";
            SceneManager.LoadScene(sceneChange);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            repositionObject = collision.gameObject;
            gameManager.SetObjectMove(positionMove, repositionObject);
            showText = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            showText = false;
        }
    }

    private void ShowText()
    {
        if (showText)
        {
            text.text = textChange.ReplacePlaceholders("{INTERACT}", input.GetUsingGamepad());
            text.spriteAsset = textChange.selectAsset;
        }
        else
        {
            text.text = "";
        }
    }
}
