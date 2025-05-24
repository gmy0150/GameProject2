using BehaviorTree;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitSecond", menuName = "BehaviorTree/ActionNode/WaitSecond")]
public class WaitSecond : Node
{
    public override void SetRunner(Enemy runner)
    {
        base.SetRunner(runner);
        ArroundTimer = 0;
    }

    float ArroundTimer;
    float SwitchTimer;
    float rotationSpeed = 1;
    bool lookingRight = true;
    public float Timer = 6;
    public float lookAngle = 45;
    public float switchTime = 3;
    public override NodeState Evaluate()
    {
        runner.UseAnim("Idle");
        runner.StopMove();

        if (runner.GetPatrol()||runner.GetProb())
        {
            ArroundTimer += Time.deltaTime;
            SwitchTimer += Time.deltaTime;
            float targetAngle = lookingRight ? lookAngle : -lookAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            runner.transform.rotation = Quaternion.Slerp(runner.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Debug.Log("AARround");
            if (SwitchTimer >= switchTime)
            {
                lookingRight = !lookingRight;
                SwitchTimer = 0;
            }
            if (ArroundTimer > Timer)
            {
                SwitchTimer = 0;
                ArroundTimer = 0;
                runner.RestartPatrol();
                runner.InitProb();
                return NodeState.SUCCESS;
            }
        }

        return NodeState.RUNNING;
    }

    public override Node Clone()
    {
        var clone = new WaitSecond();

        clone.Timer = this.Timer;
        clone.lookAngle = this.lookAngle;
        clone.switchTime = this.switchTime;
        return clone;
    }

    public override void initNode()
    {
        runner.RestartPatrol();
        ArroundTimer = 0;
        SwitchTimer = 0;
    }
}
