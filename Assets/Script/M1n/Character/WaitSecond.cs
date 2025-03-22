using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaitSecond : TestNode
{
    GuardAI GuardAI;
    public WaitSecond(GuardAI guardAI)
    {
        GuardAI = guardAI;
        ArroundTimer = 0;

    }
    float ArroundTimer;
    float SwitchTimer;
    float Timer = 6;
    float lookAngle = 45;
    float rotationSpeed = 1;
    bool lookingRight = true;
    float switchTime = 3;
    public override NodeState Evaluate()
    {
        GuardAI.StopMove();
        ArroundTimer += Time.deltaTime;
        SwitchTimer += Time.deltaTime;
        float targetAngle = lookingRight ? lookAngle : -lookAngle;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        GuardAI.transform.rotation = Quaternion.Slerp(GuardAI.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (SwitchTimer >= switchTime)
        {
            lookingRight = !lookingRight;
            SwitchTimer = 0;
        }
        if (ArroundTimer > Timer)
        {
            Debug.Log("success");
            GuardAI.RestartPatrol();
            ArroundTimer = 0;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }



}
