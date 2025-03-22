using UnityEngine;
public class MoveProbArea : TestNode
{
    GuardAI GuardAI;
    public MoveProbArea(GuardAI guardAI)
    {
        GuardAI = guardAI;
    }
    float ProbTime;

    public override NodeState Evaluate()
    {
        if (GuardAI.isEnd())
        {
            GuardAI.MoveProb(GuardAI.GetNoiseVec());
            if (GuardAI.GetProb())
            {
                ProbTime += Time.deltaTime;
                if (ProbTime > 3)
                {
                    ProbTime = 0;
                    GuardAI.InitNoise();
                    GuardAI.InitProb();
                    GuardAI.StopMove();
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
