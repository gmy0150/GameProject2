using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TChasePlayer : TestNode
{
    private GuardTest guardAI;

    public TChasePlayer(GuardTest guardAI)
    {
        this.guardAI = guardAI;
    }

    public override NodeState Evaluate()
    {
        guardAI.ChasePlayer();  // 플레이어 추적 로직
        return NodeState.RUNNING;  // 추적 중이므로 RUNNING 상태로 반환
    }
}
