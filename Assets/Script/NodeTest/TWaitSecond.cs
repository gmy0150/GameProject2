
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TWaitSecond : TestNode
{
    private GuardTest guardAI;
    private float timer;
    private float waitTime;

    public TWaitSecond(GuardTest guardAI, float waitTime)
    {
        this.guardAI = guardAI;
        this.waitTime = waitTime;
        this.timer = 0f;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime)
        {
            return NodeState.SUCCESS;  // ��ٸ� �ð��� �������� SUCCESS
        }
        return NodeState.RUNNING;  // ���� ��ٸ��� ���̹Ƿ� RUNNING
    }
}
