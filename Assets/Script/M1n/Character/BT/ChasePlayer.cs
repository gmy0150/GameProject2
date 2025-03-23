using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class ChasePlayer : Node
{
    Enemy GuardAI;
    Player player;
    public ChasePlayer(Enemy GuardAI)
    {
        this.GuardAI = GuardAI;
        this.player = GameObject.FindAnyObjectByType<Player>();
    }

    public override NodeState Evaluate()
    {
        if(player.GetHide())
        {
            GuardAI.missPlayer();
            GuardAI.StopMove();
            return NodeState.FAILURE;
        }
        GuardAI.StartChase(player);
        
        return NodeState.RUNNING;
    }

}
