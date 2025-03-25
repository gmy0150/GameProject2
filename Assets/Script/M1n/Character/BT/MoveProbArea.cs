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
            runner.MoveProb(runner.GetNoiseVec());
            if (runner.GetProb())
            {
                ProbTime += Time.deltaTime;
                if (ProbTime > 3)
                {
                    ProbTime = 0;
                    runner.InitNoise();
                    runner.InitProb();
                    runner.StopMove();
                    return NodeState.SUCCESS;
                }
            }
            else
            {
                ProbTime = 0;
            }
        }

        return NodeState.RUNNING;
    }
}
