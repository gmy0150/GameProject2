using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "Patrol", menuName = "BehaviorTree/ActionNode/Patrol")]
public class Patrol : Node
{
    public override Node Clone()
    {
        return ScriptableObject.CreateInstance<Patrol>();
    }

    public override NodeState Evaluate()
    {

        Debug.Log("patrol");
        runner.UseAnim("Move");
        if (!runner.GetPatrol())
        {
            runner.Patrols();
        }
        else
        {
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;

    }

    public override void initNode()
    {
        throw new System.NotImplementedException();
    }
}
