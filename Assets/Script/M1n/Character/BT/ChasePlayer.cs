using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
[CreateAssetMenu(fileName = "ChasePlayer", menuName = "BehaviorTree/ActionNode/ChasePlayer")]
public class ChasePlayer : Node
{

    Player player;

    public override void SetRunner(Enemy runner)
    {
        base.SetRunner(runner);
        this.player = GameObject.FindAnyObjectByType<Player>();
    }

    public override NodeState Evaluate()
    {
        if(player.GetHide())
        {
            runner.missPlayer();
            runner.StopMove();
            return NodeState.FAILURE;
        }
        runner.StartChase(player);
        
        return NodeState.RUNNING;
    }

}
