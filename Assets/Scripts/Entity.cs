using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity
{
    Dictionary<string, Action> accionsNoParam = new();
    Dictionary<string, Action<float>> accionsFloat = new();

    Rigidbody2D r;
    Transform t;
    Animator a;

    public void Configuration(Rigidbody2D rb, Transform tr, Animator ab)
    {
        this.r = rb;
        this.t = tr;
        this.a = ab;

        AddAction("moveRigth", MoveRigth);
        AddAction("moveLeft", MoveLeft);
        AddAction("watchRigth", WatchRigth);
        AddAction("watchLeft", WatchLeft);
        AddAction("attack", Attack);
        AddAction("stop", Stop);
    }

    public void Configuration(Transform tr)
    {
        this.t = tr;
        AddAction("stop", Stop);
    }

    public void AddAction(string accion, Action method)
    {
        accionsNoParam[accion] = method;
    }

    public void AddAction<T>(string accion, Action<float> method)
    {
        accionsFloat[accion] = method;
    }

    public void UpdateAction(string accion, Action method)
    {
        accionsNoParam[accion] = method; // Sobrescribe o agrega
    }

    public void UpdateAction<T>(string accion, Action<float> method)
    {
        accionsFloat[accion] = method; // Sobrescribe o agrega
    }

    public void RunAction(string accion)
    {
        string[] accionSeparate = accion.Split(',');
        string accionKey = accionSeparate[0];
        bool accionContentGome = accionSeparate.Length > 1;

        if (accionContentGome)
        {
            float var = float.Parse(accionSeparate[1]);
            if (accionsFloat.ContainsKey(accionKey))
            {
                accionsFloat[accionKey](var);
            }
        }
        else
        {
            if (accionsNoParam.ContainsKey(accionKey))
            {
                accionsNoParam[accionKey]();
            }
        }
    }

    public void MoveLeft()
    {
        float speed = 1.3f * -1.0f;
        MoveEntity(speed);
    }

    public void MoveRigth()
    {
        float speed = 1.3f * 1.0f;
        MoveEntity(speed);
    }


    public void WatchRigth()
    {
        Watch(1);
    }

    public void WatchLeft()
    {
        Watch(-1);
    }

    public void MoveEntity(float speed)
    {
        float valorScale = Mathf.Abs(t.transform.localScale.x) * Math.Sign(speed);
        t.transform.localScale = new Vector3(valorScale, t.localScale.y, t.localScale.z);
        r.linearVelocityX = speed;
        a.SetFloat("Speed", Math.Abs(speed));
    }

    public void Watch(int direction)
    {
        float valorScale = Mathf.Abs(t.transform.localScale.x) * direction;
        t.transform.localScale = new Vector3(valorScale, t.localScale.y, t.localScale.z);
    }

    public void Attack()
    {
        a.SetBool("Attack", true);
    }

    public void Stop()
    {
        if (r == null) return;
        r.linearVelocity = Vector2.zero;
        a.SetFloat("Speed", 0f);
    }
}
