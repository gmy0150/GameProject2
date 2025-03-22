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
        guardAI.ChasePlayer();  // �÷��̾� ���� ����
        return NodeState.RUNNING;  // ���� ���̹Ƿ� RUNNING ���·� ��ȯ
    }
}
