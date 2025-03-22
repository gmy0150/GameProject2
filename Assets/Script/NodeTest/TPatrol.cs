
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPatrol : TestNode
{
    private GuardTest guardAI;

    public TPatrol(GuardTest guardAI)
    {
        this.guardAI = guardAI;
    }

    public override NodeState Evaluate()
    {
        guardAI.Patrol();  // 순찰 로직
        return NodeState.RUNNING;  // 순찰 중이므로 RUNNING 상태로 반환
    }

}
