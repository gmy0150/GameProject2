using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSelector : TestNode
{
    private List<TestNode> nodes;
    public TestSelector(List<TestNode> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        foreach (var node in nodes)
        {
            NodeState state = node.Evaluate();
            if (state == NodeState.SUCCESS || state == NodeState.RUNNING)
            {
                return state;  // �ϳ��� SUCCESS�� RUNNING�� ������ �װ� ��ȯ
            }
        }
        return NodeState.FAILURE;
    }


}
