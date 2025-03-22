using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
public class Patrol : TestNode
{
    GuardAI GuardAI;
    bool first;
    public Patrol(GuardAI guardAI)
    {
        GuardAI = guardAI;
    }
    public override NodeState Evaluate()
    {

        if (!GuardAI.GetPatrol())
        {
            GuardAI.Patrols();

        }
        else
        {

            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;


    }
}
