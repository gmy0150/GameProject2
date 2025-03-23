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
    private bool isRotating = false;



    public override NodeState Evaluate()
    {
        Vector3 noiseVec = runner.GetNoiseVec();

        if (noiseVec != Vector3.zero && TempVec != noiseVec)
        {
            ArroundTimer = 0;
            TempVec = noiseVec;
            runner.InitProb();
            runner.StopMove();
        }

        if (noiseVec != Vector3.zero)
        {
            float rotationSpeed = 2;
            Quaternion currentRotation = runner.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(noiseVec - runner.transform.position);


            runner.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                isRotating = false;
                ArroundTimer += Time.deltaTime;
            }

            if (ArroundTimer >= Timer)
            {
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
}