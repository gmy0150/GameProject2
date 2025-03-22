
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
        guardAI.Patrol();  // ���� ����
        return NodeState.RUNNING;  // ���� ���̹Ƿ� RUNNING ���·� ��ȯ
    }

}
