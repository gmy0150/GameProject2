using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveProbArea", menuName = "BehaviorTree/ActionNode/MoveProbArea")]
public class MoveProbArea : Node
{


    float ProbTime;

    public override Node Clone()
    {
        return new MoveProbArea();
    }

    public override NodeState Evaluate()
    {
        
        if (runner.isEndProb())
        {
            runner.UseAnim("Move");
            runner.MoveProb(runner.GetNoiseVec());
            if (runner.GetProb())
            {
                ProbTime += Time.time;
                if (ProbTime > 3)
                {
                    runner.ProbEnd();
                    runner.InitNoise();
                    runner.InitProb();
                    runner.StopMove();

                    ProbTime = 0;
                    curnode = this;
                    Debug.Log(curnode.GetType().Name);
                    return NodeState.SUCCESS;
                }
            }
        }

        return NodeState.RUNNING;
    }

    public override void initNode()
    {
        runner.ProbEnd();
        runner.InitNoise();
        runner.InitProb();
        runner.StopMove();

        ProbTime = 0;
    }
}
