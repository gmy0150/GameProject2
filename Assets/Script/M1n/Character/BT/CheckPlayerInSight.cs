using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
[CreateAssetMenu(fileName = "CheckSight", menuName = "BehaviorTree/ActionNode/CheckSight")]
public class CheckPlayerInSight : Node
{
    public override Node Clone()
    {
        return ScriptableObject.CreateInstance<CheckPlayerInSight>();
    }

    public override NodeState Evaluate()
    {
        runner.AboveUI("",false);
        if (runner != null)
        {
            return runner.GetPlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
        }
        Debug.Log("����");
        return NodeState.FAILURE;
    }

    public override void initNode()
    {
        throw new System.NotImplementedException();
    }
}
