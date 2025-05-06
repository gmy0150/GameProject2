using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "BehaviorTree/ActionNode/Stun")]
public class Stun : Node
{
    public override Node Clone()
    {
        var clone = CreateInstance<Stun>();
        clone.StunTimer = this.StunTimer;

        return clone;
    }
    public float StunTimer;
    float Timer;
    public override NodeState Evaluate()
    {
        // runner.UseAnim("Stun");
        if (runner.GetStun())
        {
            Timer += Time.deltaTime;
            if(Timer > StunTimer){
                runner.releaseStun();
            }
            runner.missPlayer();
            runner.InitNoise();
            runner.InitProb();
            runner.ProbEnd();
        }
        else
        {
            Timer = 0;
            return NodeState.FAILURE;
        }
        return NodeState.RUNNING;

    }


}
