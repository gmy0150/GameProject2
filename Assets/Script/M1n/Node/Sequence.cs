using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class Sequence : Node
{
    private List<Node> nodes;

    public Sequence(List<Node> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            NodeState state = node.Evaluate();
            if (state == NodeState.FAILURE)
            {
                return NodeState.FAILURE;  // �ϳ��� FAILURE�� ������ FAILURE ��ȯ
            }
            if (state == NodeState.RUNNING)
            {
                return NodeState.RUNNING;  // �ϳ��� RUNNING�̸� RUNNING ��ȯ
            }
        }
        return NodeState.SUCCESS;  // ��� �ڽ��� SUCCESS�� SUCCESS ��ȯ
    }
}
