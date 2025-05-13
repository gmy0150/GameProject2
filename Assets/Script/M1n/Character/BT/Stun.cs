using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "Stun", menuName = "BehaviorTree/ActionNode/Stun")]
public class Stun : Node
{
    public Node root;
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
        runner.AboveUI(this.GetType().Name);
        if (runner.GetStun())
        {
            Timer += Time.deltaTime;
            if(Timer > StunTimer){
                runner.releaseStun();
                initNode();
            }
        }
        else
        {
            Timer = 0;
            return NodeState.FAILURE;
        }
        return NodeState.RUNNING;

    }

    public override void initNode()
    {
        InitTree(root);
    }
}
