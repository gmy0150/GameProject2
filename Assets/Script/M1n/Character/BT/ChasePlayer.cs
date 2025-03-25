using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
[CreateAssetMenu(fileName = "ChasePlayer", menuName = "BehaviorTree/ActionNode/ChasePlayer")]
public class ChasePlayer : Node
{

    Player player;
    public float Timer;
    float MoveTime;
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
            Debug.Log("È®ÀÎ");
            Debug.Log(player.ToString());
            if (player.GetHide())
            {
                runner.missPlayer();
                runner.StopMove();
                return NodeState.FAILURE;
            }
                Debug.Log(MoveTime);
            if (MoveTime > Timer)
            {
                MoveTime = 0;
                Debug.Log("³¡");
                return NodeState.FAILURE;
            }
            runner.StartChase(player);
        }
        else
        {
            if(player.GetHide())
            {
                runner.missPlayer();
                runner.StopMove();
                return NodeState.FAILURE;
            }
            runner.StartChase(player);
        }
        
        return NodeState.RUNNING;
    }

}
