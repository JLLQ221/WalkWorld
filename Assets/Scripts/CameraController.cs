using System;
using System.Collections;
using UnityEngine;

public class CameraController : BasicActor
{
    public CameraFollow cameraFollow;
    public Transform[] objectsFollow;
    bool followActor = false;
    int positionFollow = -1;

    protected override void Awake()
    {
        base.Awake();

        entity.AddAction<float>("followObject", FollowObject);
        entity.AddAction<float>("movePosition", MoveCameraPosition);
        entity.AddAction("notFollow", StopFollow);
        entity.AddAction("stayPosition", StayPosition);
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void LateUpdate()
    {
        if (followActor)
        {
            cameraFollow.FollowObject(objectsFollow[positionFollow]);
        }
    }

    private void FollowObject(float follow)
    {
        continueStep = false;
        positionFollow = (int)follow - 1;
        if (positionFollow < objectsFollow.Length)
        {
            float objectFollow = objectsFollow[positionFollow].position.x;
            cameraFollow.NotFollowPlayer();
            StartCoroutine(StartFollow(objectFollow));
        }
    }

    private void MoveCameraPosition(float xGo)
    {
        continueStep = false;
        cameraFollow.NotFollowPlayer();
        StartCoroutine(StartFollow(xGo, false));
    }

    private void StopFollow()
    {
        followActor = false;
        positionFollow -= 1;
        cameraFollow.FollowPlayer();
    }

    private void StayPosition()
    {
        cameraFollow.NotFollowPlayer();
    }

    private IEnumerator StartFollow(float xGo, bool followObject = true)
    {
        bool continueWhile = true;
        float xOrigin = cameraFollow.transform.position.x;
        bool goRigth = Math.Sign(xOrigin - xGo) < 0;

        while (continueWhile)
        {
            float xCamera = cameraFollow.transform.position.x;
            if (xCamera >= xGo - 0.2f && xCamera <= xGo + 0.8f)
            {
                if (followObject)
                {
                    followActor = true;
                }
                continueStep = true;
                yield break;
            }

            if (goRigth)
            {
                cameraFollow.GetEntity().RunAction("moveRigth");
            }
            else
            {
                cameraFollow.GetEntity().RunAction("moveLeft");
            }
            yield return new WaitForSeconds(0.026f);
        }
    }

    public override void NormalMoving()
    {
        StopFollow();
    }
}
