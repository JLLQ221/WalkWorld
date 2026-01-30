using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeedUp : MonoBehaviour
{
    private Animator animation;
    private SpriteRenderer spriteRenderer;
    public AudioDates audios;

    public GameObject seedPlant;
    public CinematicObject cinematic;
    public Scene1Plants plants;
    public SpritesButtonChema textChange;
    public TextButtonController textShow;
    private AudioSource audio;

    private TextMeshProUGUI textKetInteract;
    private InputDetector inputDetector;
    private bool showText;
    private InputAction action;

    private GameObject plantCreate;
    private Player playerContact;
    private bool isInteract = false;

    private void Awake()
    {
        textKetInteract = GetComponentInChildren<TextMeshProUGUI>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animation = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inputDetector = FindAnyObjectByType<InputDetector>();
        action = InputSystem.actions.FindAction("Interact");
        audio = GetComponent<AudioSource>();
    }

    public void Activate()
    {
        StartCinematic();
        audio.PlayOneShot(audios.getEffect(0), 0.5f);
    }

    private void Update()
    {
        if (isInteract)
        {
            textKetInteract.text = "";
            Destroy(textKetInteract);
            return;
        }
        ShowText();
        if (showText && action.WasPressedThisFrame())
        {
            plants.activeObject = true;
            plants.showCinematic = true;
            showText = false;
            cinematic.StopPlayerActor();
            if (textShow != null)
            {
                textShow.ActiveText();
            }
            if (plantCreate == null)
            {
                plantCreate = Instantiate(seedPlant, transform.position, Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInteract) { return; }
        MoveObject obj = collision.GetComponent<MoveObject>();
        if (playerContact == null)
        {
            playerContact = collision.GetComponent<Player>();
            if (playerContact.GetSeddCount <= 0)
            {
                playerContact = null;
                return;
            }
        }
        if (obj != null)
        {
            Activate();
        }
        else if (playerContact != null)
        {
            showText = true;
        }
    }

    IEnumerator CallCinematic()
    {
        float timeWait = 2.8f;
        float time = 0;

        while (time < timeWait)
        {
            time += Time.deltaTime;
            yield return null;
        }

        cinematic.RunActions();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == playerContact && !isInteract)
        {
            showText = false;
        }
    }
    private void ShowText()
    {
        if (showText && !isInteract)
        {
            textKetInteract.text = textChange.ReplacePlaceholders("{INTERACT}", inputDetector.GetUsingGamepad());
            textKetInteract.spriteAsset = textChange.selectAsset;
        }
        else
        {
            textKetInteract.text = "";
        }
    }

    private void StartCinematic()
    {
        if(textShow != null)
        {
        textShow.Desactive();
        }
        animation.enabled = true;
        spriteRenderer.enabled = true;
        plantCreate.GetComponent<MoveObject>().DestroyAll();
        playerContact.AddSedd(-1);
        isInteract = true;
        StartCoroutine(CallCinematic());
    }
}
