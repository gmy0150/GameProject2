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
    public float Timer = 6;
    public float lookAngle = 45;
    float rotationSpeed = 1;
    bool lookingRight = true;
    public float switchTime = 3;
    public override NodeState Evaluate()
    {
        runner.StopMove();
        ArroundTimer += Time.deltaTime;
        SwitchTimer += Time.deltaTime;
        float targetAngle = lookingRight ? lookAngle : -lookAngle;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        runner.transform.rotation = Quaternion.Slerp(runner.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (SwitchTimer >= switchTime)
        {
            lookingRight = !lookingRight;
            SwitchTimer = 0;
        }
        if (ArroundTimer > Timer)
        {
            Debug.Log("success");
            runner.RestartPatrol();
            ArroundTimer = 0;
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }



}
