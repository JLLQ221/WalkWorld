using System.Collections;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPZController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private RectTransform transformReal;
    public TextMeshProUGUI pzText;
    private float moveForce = 20;
    private int countCall = 0;
    protected bool stopCourotina = false;
    public float amplitude = 5f;   // qué tanto sube/baja
    public float frequency = 2f;   // velocidad de la onda
    private int numberPz = 0;
    private int maxPz = 0;
    private Coroutine outCourotine = null;

    private Mesh mesh;
    private Vector3[] vertices;
    void Start()
    {
        transformReal = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ShakeText()
    {
        pzText.ForceMeshUpdate();
        mesh = pzText.mesh;
        vertices = mesh.vertices;

        TMP_TextInfo textInfo = pzText.textInfo;

        for (int i = textInfo.characterCount - 1; i < textInfo.characterCount; i++)
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

    public void CallCard(int addPz)
    {
        maxPz += addPz;
        StartCoroutine(AddPzCourotine(addPz));
        MoveCard();
    }

    public void MoveCard()
    {
        float x = transformReal.anchoredPosition.x;
        countCall++;
        if (countCall > 1)
        {
            if (outCourotine != null)
            {
                stopCourotina = true;
            }
            return;
        }
        if (x > 0)
        {
            StartCoroutine(MoveInCorutine());
        }
    }

    public void ChangeText(int pz)
    {
        pzText.text = $"Piezas: <b>{pz}</b>";
    }

    IEnumerator AddPzCourotine(int endFor)
    {
        float multiplicateCall = countCall * 0.4f;
        yield return new WaitForSeconds(1.3f + multiplicateCall);
        for (int i = 0; i < endFor; i++)
        {
            yield return new WaitForSeconds(1.2f + (multiplicateCall));
            numberPz++;
            ChangeText(numberPz);
        }
        if (numberPz == maxPz && outCourotine == null)
        {
            outCourotine = StartCoroutine(MoveOutCorutine());
        }
    }

    private float MoveCardDirection(float position)
    {
        float x = transformReal.anchoredPosition.x + position;
        transformReal.anchoredPosition = new Vector3(x, 0, 0);
        return x;
    }

    IEnumerator MoveOutCorutine(float xExit = 300)
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            float x = MoveCardDirection(moveForce);
            if (x == xExit)
            {
                countCall = 0;
                yield break;
            }
            else if (stopCourotina)
            {
                if (x > 0)
                {
                    StartCoroutine(MoveInCorutine());
                }
                else
                {
                    outCourotine = null;
                }
                stopCourotina = false;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator MoveInCorutine(float xExit = 0)
    {
        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            float x = MoveCardDirection(-moveForce);
            if (x == xExit)
            {
                countCall = 0;
                stopCourotina = false;
                outCourotine = null;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
