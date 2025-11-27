using System.Collections;
using UnityEngine;

public class BasicActor : MonoBehaviour
{
    protected Entity entity;
    protected bool freeMove = false;
    protected bool continueStep = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        var anim = GetComponent<Animator>();
        var tr = transform;

        entity = new Entity();

        if (rb != null && anim != null)
        {
            entity.Configuration(rb, tr, anim);
            entity.AddAction<float>("movePosition", MovePosition);
        }
        else
        {
            entity.Configuration(tr);
        }
    }

    public void MovePosition(float xGo)
    {
        continueStep = false;
        StartCoroutine(WalkPosition(xGo)); // pasa el parámetro
    }

    public IEnumerator WalkPosition(float xGo)
    {
        bool continueWhile = true;
        float xOrigin = transform.position.x;
        bool isWalkRigth = Mathf.Sign(xOrigin - xGo) < 0;

        while (continueWhile)
        {
            float x = transform.position.x;
            if (x >= xGo - 0.2f && x <= xGo + 0.5f)
            {
                entity.RunAction("stop");
                continueStep = true;
                yield break;
            }

            if (isWalkRigth)
            {
                entity.RunAction("moveRigth");
            }
            else
            {
                entity.RunAction("moveLeft");
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetFreeMoving(bool value)
    {
        freeMove = value;
    }

    public virtual void NormalMoving()
    {

    }

    public bool GetFreeMovie() => freeMove;

    public bool GetContinue() => continueStep;

    public Entity GetEntity() => entity;
}
