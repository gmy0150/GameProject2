using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[CreateAssetMenu(fileName = "SequenceNode", menuName = "BehaviorTree/Sequence")]
public class Sequence : CompositeNode
{


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
