using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CompositeNode : Node
{
    [SerializeField] public Node[] nodes;

    public override Node Clone()
    {
        CompositeNode newSequence = Instantiate(this);
        newSequence.nodes = new Node[nodes.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            newSequence.nodes[i] = nodes[i].Clone(); 
        }

        return newSequence;
    }

    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }

}
