using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class Selector : Node
{
    private List<Node> nodes;
    public Selector(List<Node> nodes)
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
                return state;  // 하나라도 SUCCESS나 RUNNING이 있으면 그걸 반환
            }
        }
        return NodeState.FAILURE;
    }


}
