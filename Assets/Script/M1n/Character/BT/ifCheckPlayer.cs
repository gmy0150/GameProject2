using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "Alert", menuName = "BehaviorTree/ActionNode/Alert")]

public class ifCheckPlayer : Node
{
    AlertSystem alertSystem;

    private void OnEnable()
    {
        // 여기서 FindAnyObjectByType을 호출하면 정상 작동함
        alertSystem = FindAnyObjectByType<AlertSystem>();
    }
    public override NodeState Evaluate()
    {
        alertSystem.WorkLight();
        return NodeState.RUNNING;
    }

    

}
