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
            Debug.Log(runner.isEndProb());
        if (runner.isEndProb())
        {
            Debug.Log(111);
            runner.UseAnim("Move");
            runner.MoveProb(runner.GetNoiseVec());
            if (runner.GetProb())
            {
                ProbTime += Time.time;
                Debug.Log(ProbTime);
                if (ProbTime > 3)
                {
                    runner.ProbEnd();
                    runner.InitNoise();
                    runner.InitProb();
                    runner.StopMove();

                    Debug.Log("작");
                    ProbTime = 0;
                    return NodeState.SUCCESS;
                }
            }
        }

        return NodeState.RUNNING;
    }
}
