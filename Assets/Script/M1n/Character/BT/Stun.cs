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
        clone.releaseTimer = this.releaseTimer;

        return clone;
    }
    public float StunTimer;
    float Timer;
    public float releaseTimer;
    public override NodeState Evaluate()
    {
        // runner.UseAnim("Stun");
        if (runner.GetStun())
        {
            runner.AboveUI(this.GetType().Name);
                runner.UseAnim("");
            // runner.UseAnim(this.GetType().Name);
            Timer += Time.deltaTime;
            if (Timer > StunTimer)
            {
                runner.InitNoise();
                Debug.Log("??");
                runner.anim.SetTrigger("StandUp");
                runner.releaseStun(releaseTimer);
                initNode();
            }
            else
            {
                
                runner.anim.SetTrigger("Stun");
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
