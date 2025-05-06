using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "CCTVMove", menuName = "BehaviorTree/ActionNode/CCTVMove")]
public class CCTVMove : Node
{

    float ArroundTimer;
    float SwitchTimer;
    bool lookingRight = true;
    float initYRotation;
    public float switchTime = 3;
    public float Timer = 6;
    public float lookAngle = 30;
    public float rotationSpeed = 3;


    public override void SetRunner(Enemy runner)
    {
        base.SetRunner(runner);
        initYRotation = runner.transform.eulerAngles.y;


    }
    public override NodeState Evaluate()
    {
        if (runner == null) return NodeState.FAILURE;  // runner�� null�� ��� FAILURE ��ȯ

        ArroundTimer += Time.deltaTime;
        SwitchTimer += Time.deltaTime;

        float targetAngle = initYRotation + (lookingRight ? lookAngle : -lookAngle);
        Quaternion targetRotation = Quaternion.Euler(runner.transform.eulerAngles.x, targetAngle, runner.transform.eulerAngles.z);
        float t = 1 - Mathf.Exp(-rotationSpeed * Time.deltaTime);
        runner.transform.rotation = Quaternion.Slerp(runner.transform.rotation, targetRotation, t);



        if (SwitchTimer >= switchTime)
        {
            lookingRight = !lookingRight;
            SwitchTimer = 0;
        }
        if (ArroundTimer > Timer)
        {
            ArroundTimer = 0;
            SwitchTimer = 0;
            lookingRight = !lookingRight;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }

    public override Node Clone()
    {
        var clone = CreateInstance<CCTVMove>();
        clone.Timer = this.Timer;
        clone.lookAngle = this.lookAngle;
        clone.rotationSpeed = this.rotationSpeed;
        clone.switchTime = this.switchTime;

        return clone;
    }

    public override void initNode()
    {
        ArroundTimer = 0;
        SwitchTimer = 0;
        lookingRight = false;
    }
}
