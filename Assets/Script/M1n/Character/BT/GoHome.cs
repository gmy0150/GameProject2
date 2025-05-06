using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "GoHome", menuName = "BehaviorTree/ActionNode/GoHome")]
public class GoHome : Node
{
    Vector3 position;

    public override Node Clone()
    {
        return ScriptableObject.CreateInstance<GoHome>();
    }
    public override void SetRunner(Enemy runner)
    {
        base.SetRunner(runner);
        runner.SetHome(runner.transform.position);
    }

    public override NodeState Evaluate()
    {
        if (runner.GetHome())
        {
            runner.StopMove();
            runner.HomeArrive();
            return NodeState.SUCCESS;
        }
        else
        {
            if (!runner.IsHome())
            {
                Debug.Log("������?");
                runner.MoveHome();

            }
            return NodeState.RUNNING;
        }



    }

    public override void initNode()
    {
        throw new System.NotImplementedException();
    }
}
