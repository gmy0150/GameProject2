using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
[CreateAssetMenu(fileName = "ChasePlayer", menuName = "BehaviorTree/ActionNode/ChasePlayer")]
public class ChasePlayer : Node
{
    Player player;
    float MoveTime;
    float BarkTime;
    public float Timer;
    public float BarkTimer;

    public override void SetRunner(Enemy runner)
    {
        base.SetRunner(runner);
        this.player = GameObject.FindAnyObjectByType<Player>();
        MoveTime = 0;
    }

    public override NodeState Evaluate()
    {
        runner.UseAnim("ChasePlayer");
        runner.AboveUI(this.GetType().Name);
        

        
            if (player.GetInterActControll().GetHide())
            {
                runner.missPlayer();
                runner.StopMove();
                return NodeState.FAILURE;
            }
            runner.StartChase(player);
        
        
        return NodeState.RUNNING;
    }

    public override Node Clone()
    {
        ChasePlayer clone = CreateInstance<ChasePlayer>();
        clone.Timer = this.Timer;
        clone.BarkTimer = this.BarkTimer;   

        return clone;
    }

    public override void initNode()
    {
        MoveTime = 0;
        BarkTime = 0;
        runner.missPlayer();
        runner.StopMove();
    }
}
