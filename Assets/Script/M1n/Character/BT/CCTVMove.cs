using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CCTVMove : Node
{
    Enemy GuardAI;
    public CCTVMove(Enemy guardAI)
    {
        GuardAI = guardAI;
        ArroundTimer = 0;
        SwitchTimer = 0;
        initYRotation = GuardAI.transform.eulerAngles.y;
    }
    float ArroundTimer;
    float SwitchTimer;
    float Timer = 6;
    float lookAngle = 30;
    float rotationSpeed = 1;
    bool lookingRight = true;
    float switchTime = 3;
    float initYRotation;
    public override NodeState Evaluate()
    {
        ArroundTimer += Time.deltaTime;
        SwitchTimer += Time.deltaTime;

        float targetAngle = initYRotation + (lookingRight ? lookAngle : -lookAngle);
        Quaternion targetRotation = Quaternion.Euler(GuardAI.transform.eulerAngles.x, targetAngle, GuardAI.transform.eulerAngles.z);
        float t = 1 - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        GuardAI.transform.rotation = Quaternion.Slerp(GuardAI.transform.rotation, targetRotation, t);

        if (SwitchTimer >= switchTime)
        {
            lookingRight = !lookingRight;
            SwitchTimer = 0;
        }
        if (ArroundTimer > Timer)
        {
            ArroundTimer = 0;
            SwitchTimer = 0;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

}
