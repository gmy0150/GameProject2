
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
        // �÷��̾ �þ߿� ������ SUCCESS, ������ FAILURE
        return guardAI.IsPlayerInSight() ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
