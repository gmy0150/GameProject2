using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckPlayerInSight : Node
{
    Enemy GuardAI;
    public CheckPlayerInSight(Enemy guardAI)
    {
        GuardAI = guardAI;
    }
    public override NodeState Evaluate()
    {
        return GuardAI.GetPlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
