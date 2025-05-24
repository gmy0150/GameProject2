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
        if (runner != null)
        {
            if (runner.GetDetectPlayer())
            {
                runner.AboveUI("CheckNoise", true);
                if (runner.GetPlayer())
                {
                    return NodeState.SUCCESS;
                }
                else
                {
                runner.StopMove();
                    runner.gameObject.transform.LookAt(runner.player.transform.position);
                    
                }
                return NodeState.RUNNING;
            }
        }
        return NodeState.FAILURE;
    }

    public override void initNode()
    {
        
    }
}
