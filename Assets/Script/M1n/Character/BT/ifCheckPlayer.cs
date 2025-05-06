using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "Alert", menuName = "BehaviorTree/ActionNode/Alert")]

public class ifCheckPlayer : Node
{
    //AlertSystem alertSystem;

    private void OnEnable()
    {
        // ���⼭ FindAnyObjectByType�� ȣ���ϸ� ���� �۵���
        //alertSystem = FindAnyObjectByType<AlertSystem>();
    }
    public override NodeState Evaluate()
    {
        //alertSystem.WorkLight();
        return NodeState.RUNNING;
    }

    public override Node Clone()
    {
        return new ifCheckPlayer();
    }

    public override void initNode()
    {
        throw new System.NotImplementedException();
    }
}
