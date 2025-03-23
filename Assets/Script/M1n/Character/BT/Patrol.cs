using BehaviorTree;
public class Patrol : Node
{
    Enemy GuardAI;
    public Patrol(Enemy guardAI)
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
