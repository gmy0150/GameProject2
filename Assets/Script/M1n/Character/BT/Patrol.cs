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
        runner.AboveUI("", false);

        runner.UseAnim("Move");
        if (!runner.GetPatrol())
        {
            runner.AboveUI("",false);
            runner.Patrols();
        }
        else
        {
            Debug.Log("cur");

            return NodeState.SUCCESS;
        }
        return NodeState.RUNNING;

    }

    public override void initNode()
    {
        
    }
}
