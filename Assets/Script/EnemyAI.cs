using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        return MoveSpeed;
    }
    public float detectAngle = 90f;   // 시야각 (부채꼴)
    public float detectionDistance = 10f; // 감지 거리
    public int segments = 20;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = segments + 3;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);  // 감지 범위 (원형)

        Vector3 leftBoundary = Quaternion.Euler(0, -detectAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, detectAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * detectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * detectionDistance);
    }
    INode SettingBT()
    {
        return new SelectorNode(new List<INode>()
        {
            new SequenceNode(new List<INode>()
            {
                new ActionNode(CheckDetectPlayer),
                new ActionNode(ChasePlayer),
            }),
            new SequenceNode(new List<INode>()
            {
                new ActionNode(FollowPath),
                new ActionNode(WaitAtPoint)
            })
        });
    }

    private INode.ENodeState WaitAtPoint()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState FollowPath()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState ChasePlayer()
    {
        throw new NotImplementedException();
    }

    private INode.ENodeState CheckDetectPlayer()
    {
        Transform enemyTrnas = this.transform;

        GameObject player = GameObject.FindAnyObjectByType<Player>().gameObject;
        if (player == null) return INode.ENodeState.ENS_Failure;

        Vector3 directionToPlayer = (player.transform.position - enemyTrnas.position).normalized;
        float distanceTOPlayer = Vector3.Distance(enemyTrnas.position, player.transform.position);

        if(distanceTOPlayer > detectionDistance)
            return INode.ENodeState.ENS_Failure;

        float angle = Vector3.Angle(enemyTrnas.forward, directionToPlayer);
        if ((angle > detectAngle / 2f))
            return INode.ENodeState.ENS_Failure;

        RaycastHit hit;
        if (Physics.Raycast(enemyTrnas.position, directionToPlayer, out hit, detectionDistance))
        {
            if (hit.collider.GetComponent<Player>())
            {
                return INode.ENodeState.ENS_Success;
            }
            else
            {
                return INode.ENodeState.ENS_Failure;
            }
        }
        return INode.ENodeState.ENS_Failure;
    }
}
