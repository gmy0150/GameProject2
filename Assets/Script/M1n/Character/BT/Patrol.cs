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

            Debug.Log("작동중");
        runner.UseAnim("Move");
        if (!runner.GetPatrol())
        {
            runner.Patrols();
            Debug.Log("작동중");
        }
        else
        {
            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;


    }
}
