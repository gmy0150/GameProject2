using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPlayerInSight : TestNode
{
    GuardAI GuardAI;
    public CheckPlayerInSight(GuardAI guardAI)
    {
        GuardAI = guardAI;
    }
    public override NodeState Evaluate()
    {
        return GuardAI.GetPlayer() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
