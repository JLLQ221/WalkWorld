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

    protected virtual void MovePosition(float xGo)
    {
        continueStep = false;
        StartCoroutine(WalkPosition(xGo)); // pasa el parámetro
    }

    public IEnumerator WalkPosition(float xGo)
    {
        bool continueWhile = true;
        float xOrigin = transform.position.x;
        bool isWalkRight = xGo > xOrigin;

        while (continueWhile)
        {
            float x = transform.position.x;
            if (x >= xGo - 0.2f && x <= xGo + 0.5f)
            {
                entity.RunAction("stop");
                continueStep = true;
                yield break;
            }

            if (isWalkRight)
            {
                entity.RunAction("moveRigth");
            }
            else
            {
                entity.RunAction("moveLeft");
            }
            float time = 0;
            float timeEnd = 0.1f;
            while (time < timeEnd)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    public void SetFreeMoving(bool value)
    {
        freeMove = value;
    }

    public virtual void NormalMoving()
    {

    }
    public void SetContinue(bool a) => continueStep = a;
    public bool GetFreeMovie() => freeMove;
    public bool GetContinueStep() => continueStep;
    public Entity GetEntity() => entity;
}
