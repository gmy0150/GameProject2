using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChasePlayer : TestNode
{
    GuardAI GuardAI;
    Player player;
    public ChasePlayer(GuardAI GuardAI)
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
