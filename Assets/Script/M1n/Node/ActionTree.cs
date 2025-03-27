using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

[CreateAssetMenu(fileName = "ActionTree", menuName = "BehaviorTree/custom/Action")]
public class ActionTree : ScriptableObject
{
    public Node rootNode;

    public void EvaluateTree()
    {
        Node.NodeState state = rootNode.Evaluate();
        Debug.Log("Tree Evaluated: " + state);
    }
}
