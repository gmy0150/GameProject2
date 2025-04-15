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
        if (runner.GetType() == typeof(GuardDog))
        {
            MoveTime += Time.deltaTime;
            BarkTime += Time.deltaTime;
            Debug.Log(player.ToString());
            if (BarkTime > BarkTimer)
            {
                runner.MakeNoise(runner.gameObject,10,10);
                Debug.Log("Â¢¾î");
                BarkTime = 0;
            }
            if (player.GetInterActControll().GetHide())
            {
                runner.missPlayer();
                runner.StopMove();
                return NodeState.FAILURE;
            }
            if (MoveTime > Timer)
            {
                runner.missPlayer();
                
                runner.StopMove();
                Debug.Log("³¡");
                MoveTime = 0;
                return NodeState.FAILURE;
            }
            runner.StartChase(player);
        }
        else
        {
            if(player.GetInterActControll().GetHide())
            {
                runner.missPlayer();
                runner.StopMove();
                return NodeState.FAILURE;
            }
            runner.StartChase(player);
        }
        
        return NodeState.RUNNING;
    }

    public override Node Clone()
    {
        ChasePlayer clone = CreateInstance<ChasePlayer>();
        clone.Timer = this.Timer;
        clone.BarkTimer = this.BarkTimer;   

        return clone;
    }
}
