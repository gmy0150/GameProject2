using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class CompositeNode : Node
{
    [SerializeField] public Node[] nodes;


    public override NodeState Evaluate()
    {
        throw new System.NotImplementedException();
    }

}
