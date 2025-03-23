using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
[CreateAssetMenu(fileName = "CheckSight", menuName = "BehaviorTree/ActionNode/CheckSight")]
public class CheckPlayerInSight : Node
{

    public override NodeState Evaluate()
    {
        if (runner != null)
        {
            return runner.GetPlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        Debug.Log("¾ø´Ù");
        return NodeState.FAILURE;
    }
}
