using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SelectorNode : INode
{
    List<INode> _childs;
    public SelectorNode(List<INode> childs)
    {
        _childs = childs;
    }

    public INode.ENodeState Evaluate()
    {
        if(_childs == null)
            return INode.ENodeState.ENS_Failure;
        foreach(var child in _childs)
        {
            switch (child.Evaluate())
            {
                case INode.ENodeState.ENS_Running:
                    return INode.ENodeState.ENS_Running;
                case INode.ENodeState.ENS_Success:
                    return INode.ENodeState.ENS_Success;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }

   
}
