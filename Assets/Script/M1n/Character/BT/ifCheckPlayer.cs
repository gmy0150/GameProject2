using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class ifCheckPlayer : Node
{
    Enemy Enemy;
    AlertSystem alertSystem;
    public ifCheckPlayer(Enemy enemy) {
        Enemy = enemy;
        alertSystem = GameObject.FindAnyObjectByType<AlertSystem>();
    }

    public override NodeState Evaluate()
    {
        alertSystem.WorkLight();
        return NodeState.RUNNING;
    }

    

}
