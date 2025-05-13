using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using BehaviorTree;
[CreateAssetMenu(fileName = "CheckNoise", menuName = "BehaviorTree/ActionNode/CheckNoise")]
public class CheckNoise : Node
{

    private float ArroundTimer = 0f;
    private float Timer = 1f;
    private Vector3 TempVec = Vector3.zero;
    bool hasReachedAngle = false;
    bool isFirst = false;

    public override Node Clone()
    {
        return new CheckNoise();
    }
    public override NodeState Evaluate()
    {
        
        Vector3 noiseVec = runner.GetNoiseVec();
        if (runner.isEndProb())
        {
            return NodeState.SUCCESS;
        }

        if (noiseVec != Vector3.zero && !isFirst && !runner.isEndProb())
        {
            isFirst = true;
            ArroundTimer = 0;
            runner.StartProb();
            runner.InitProb();
            runner.StopMove();
        }

        if (noiseVec != Vector3.zero && !runner.isEndProb() && runner.isProb())
        {
            float rotationSpeed = 2;
            Quaternion currentRotation = runner.transform.rotation;
            noiseVec.y = runner.transform.position.y;
            Quaternion targetRotation = Quaternion.LookRotation(noiseVec - runner.transform.position);

            Debug.Log("의심지역 발생");
            runner.AboveUI(this.GetType().Name);
            runner.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (!hasReachedAngle && Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                hasReachedAngle = true;
            }
            if (hasReachedAngle)
            {
                ArroundTimer += Time.deltaTime;
            }
            if (ArroundTimer >= Timer)
            {
                hasReachedAngle = false;
                isFirst = false;
                runner.EndProbarea();
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.FAILURE;
        }

    }

    public override void initNode()
    {
        ArroundTimer = 0f;
        TempVec = Vector3.zero;
        hasReachedAngle = false;
        isFirst = false;
        runner.EndProbarea();
    }
}