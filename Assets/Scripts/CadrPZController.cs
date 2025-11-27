using System.Collections;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class CadrPZController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RectTransform transformReal;
    public TextMeshProUGUI pzText;
    private float moveForce = 20;
    private int countCall = 0;
    protected bool stopCourotina = false;
    public float amplitude = 5f;   // qué tanto sube/baja
    public float frequency = 2f;   // velocidad de la onda

    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        transformReal = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        pzText.ForceMeshUpdate();
        mesh = pzText.mesh;
        vertices = mesh.vertices;

        TMP_TextInfo textInfo = pzText.textInfo;

        for (int i = textInfo.characterCount - 1; i < textInfo.characterCount ; i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            Vector3 offset = Vector3.up * Mathf.Sin(Time.time * frequency + i) * amplitude;

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        mesh.vertices = vertices;
        pzText.canvasRenderer.SetMesh(mesh);
    }

    public void MoveCard()
    {
        float x = transformReal.anchoredPosition.x;
        countCall++;
        if (countCall > 2) stopCourotina = true;
        if (x == 0)
        {
            StartCoroutine(MoveOutCorutine(moveForce, 300));
        }
        else
        {
            StartCoroutine(MoveInCorutine(-moveForce, 0));
        }
    }

    public void ChangeText(string text)
    {
        pzText.text = $"Piezas : <b>{text}</b>";
    }

    private float MoveCardDirection(float position)
    {
        float x = transformReal.anchoredPosition.x;
        transformReal.anchoredPosition = new Vector3(x + position, 0, 0);
        return x;
    }

    IEnumerator MoveOutCorutine(float direction, float xExit)
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            float x = MoveCardDirection(direction);
            if (x >= xExit || stopCourotina)
            {
                countCall = 0;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MoveInCorutine(float direction, float xExit)
    {
        yield return new WaitForSeconds(2f);
        while (true)
        {
            float x = MoveCardDirection(direction);
            if (x <= xExit)
            {
                countCall = 0;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
