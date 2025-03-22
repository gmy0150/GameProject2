
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TCheckPlayer : TestNode
{
    private GuardTest guardAI;

    public TCheckPlayer(GuardTest guardAI)
    {
        this.guardAI = guardAI;
    }

    public override NodeState Evaluate()
    {
        // 플레이어가 시야에 있으면 SUCCESS, 없으면 FAILURE
        return guardAI.IsPlayerInSight() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
