using System.Collections;
using TMPro;
using UnityEngine;

public class TextPoints : MonoBehaviour
{
    private RectTransform reactTransform;
    public TextMeshProUGUI textPoint;
    private Player player;
    private int points;
    private float forceX = 0;
    private float forceY = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        reactTransform = GetComponent<RectTransform>();
        MoveCount();
    }

    private void MoveCount()
    {
        float xReal = reactTransform.anchoredPosition.x;
        float x = Mathf.Abs(reactTransform.anchoredPosition.x) + (xReal < 0 ? 70f : -(xReal * 0.68f));
        float y = Mathf.Abs(reactTransform.anchoredPosition.y);
        float resultX = Mathf.Abs(x / 860);
        float resultY = Mathf.Abs(y / 490);
        forceX = xReal >= -200 && xReal <= 100 ? 0.5f : resultX;
        forceY = y != 0 ? resultY : 0.5f;
        StartCoroutine(MoveCountCourotine());
    }

    public void SetText(int points)
    {
        this.points = points;
        float sign = Mathf.Sign(points);
        int rando = Random.Range(0, 1);
        textPoint.text = $"<sprite={rando}>";
    }

    private void AddPoint()
    {
        player.AddPz(points);
    }

    private void Destroy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    IEnumerator MoveCountCourotine()
    {
        int count = 0;
        bool addPoints = true;
        float xOrigin = reactTransform.anchoredPosition.x;
        while (true)
        {
            float x = reactTransform.anchoredPosition.x;
            float y = reactTransform.anchoredPosition.y;

            bool continueX = true;
            bool continueY = true;

            if (x > 870 && x < 910)
            {
                continueX = false;
            }

            if (y > 470 && y < 510)
            {
                continueY = false;
            }

            if (y > 100 && x > 100 && addPoints)
            {
                AddPoint();
                addPoints = false;
            }

            if (!continueX && !continueY)
            {
                textPoint.color = new Color(textPoint.color.r, textPoint.color.g, textPoint.color.b, 0f);
                Destroy();
                yield break;
            }

            float xStep = continueX ? x + (forceX * ((xOrigin <= 0) ? 14f : (forceX >= 0.000 && forceX <= 0.099f) ? 100f : 13f)) : x;
            float yStep = continueY ? y + (forceY * 12f) : y;

            textPoint.color = new Color(textPoint.color.r, textPoint.color.g, textPoint.color.b, 1f - (count * forceY * 0.02f));
            reactTransform.anchoredPosition = new Vector2(xStep, yStep);
            count++;

            float time = 0;
            float timeEnd = 0.01f;
            while (time < timeEnd)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}
