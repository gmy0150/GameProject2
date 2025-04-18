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
                return NodeState.FAILURE;  // 하나라도 FAILURE가 있으면 FAILURE 반환
            }
            if (state == NodeState.RUNNING)
            {
                return NodeState.RUNNING;  // 하나라도 RUNNING이면 RUNNING 반환
            }
        }
        return NodeState.SUCCESS;  // 모든 자식이 SUCCESS면 SUCCESS 반환
    }
}
