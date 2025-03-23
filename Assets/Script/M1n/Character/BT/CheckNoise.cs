using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using BehaviorTree;
public class CheckNoise : Node
{
    private Enemy GuardAI;
    private float ArroundTimer = 0f;
    private float Timer = 1f;
    private Vector3 TempVec = Vector3.zero;
    private bool isRotating = false;

    public CheckNoise(Enemy guardAI)
    {
        GuardAI = guardAI;
    }

    public override NodeState Evaluate()
    {

        Vector3 noiseVec = GuardAI.GetNoiseVec();

        if (noiseVec != Vector3.zero && TempVec != noiseVec)
        {
            ArroundTimer = 0;
            TempVec = noiseVec;
            GuardAI.InitProb();
            GuardAI.StopMove();
        }

        if (noiseVec != Vector3.zero)
        {
            float rotationSpeed = 2;
            Quaternion currentRotation = GuardAI.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(noiseVec - GuardAI.transform.position);


            GuardAI.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(currentRotation, targetRotation) < 1f)
            {
                isRotating = false;
                ArroundTimer += Time.deltaTime;
            }

            if (ArroundTimer >= Timer)
            {
                GuardAI.EndProbarea();
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