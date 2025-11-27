using TMPro;
using UnityEngine;

public class LifeUI : MonoBehaviour
{
    public TextMeshProUGUI textLife;
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        float life = player.getLife();
        if (textLife == null) { return; }
        textLife.text = life.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (textLife == null) { return; }
        float life = player.getLife();
        textLife.text = life.ToString();
    }
}
