using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FilledUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI numberDash;
    public Image item;
    public Image filled;
    private Player player;

    private Color colorOrigin;

    bool reload;

    void Start()
    {
        numberDash.text = player.maxDash.ToString();
    }

    private void Awake()
    {
        player = FindAnyObjectByType<Player>();
        colorOrigin = item.color;
    }

    // Update is called once per frame
    void Update()
    {
        int numerDash = player.numberDash;
        reload = player.dashReloadOff;
        if (numerDash > 0)
        {
            numberDash.text = player.numberDash.ToString();
        }
        else
        {
            numberDash.text = "";
        }
    }

    private void FixedUpdate()
    {
        if (reload)
        {
            StartCoroutine(ReloadAnimation());
            item.color = new Color(colorOrigin.r, colorOrigin.g, colorOrigin.b, 0.45f);
        }
        else
        {
            filled.fillAmount = 0;
            item.color = new Color(colorOrigin.r, colorOrigin.g, colorOrigin.b, 1.0f);
        }
    }

    IEnumerator ReloadAnimation()
    {
        yield return new WaitForSeconds(0.0001f);
        filled.fillAmount += 0.018f;
    }
}
