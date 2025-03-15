using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAI : Character
{
    public Vector3[] wayPoints;
    Vector3 curPosition;
    int wayPointIndex = 0;
    float speed = 3f;
    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        return MoveSpeed;
    }
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // ��ä�� ����
    public float Distance = 5f;   // ��ä�� ������
    TestOne fanRenderer;
    void Start()
    {
        //wayPoints = new Vector3[3];

        fanRenderer = GetComponentInChildren<TestOne>();
    }
    BehaviorTreeRunner _BTRunner = null;

    void Update()
    {
        _BTRunner.Operate();
    }
    private void Awake()
    {
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }


    // ����� �׸��� (���� ����)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Distance);

        Vector3 left = Quaternion.Euler(0, -RadiusAngle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, RadiusAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * Distance);
        Gizmos.DrawLine(transform.position, transform.position + right * Distance);
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
        return INode.ENodeState.ENS_Success;
        throw new NotImplementedException();
    }

    private INode.ENodeState FollowPath()
    {
        curPosition = transform.position;
        if (wayPointIndex < wayPoints.Length)
        {
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(curPosition, wayPoints[wayPointIndex], step);

            Vector3 direction = (wayPoints[wayPointIndex] - curPosition).normalized;
            if (direction != Vector3.zero) // ȸ���� �ʿ䰡 ���� ���� ����
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 30 * Time.deltaTime);
            }

            // ��ǥ ������ �����ϸ� ���� ��������Ʈ�� �̵�
            if (Vector3.Distance(wayPoints[wayPointIndex], curPosition) < 0.1f)
            {
                wayPointIndex++;
            }

            return INode.ENodeState.ENS_Running; // ��θ� ���󰡴� ��
        }
        return INode.ENodeState.ENS_Success;
    }

    private INode.ENodeState ChasePlayer()
    {
        Player player = GameObject.FindAnyObjectByType<Player>();
        
        if(player == null) return INode.ENodeState.ENS_Failure;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if(distanceToPlayer < (Distance *1.5f))
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * (MoveSpeed * 2 )* Time.deltaTime;
            return INode.ENodeState.ENS_Running;
        }

        DetectPlayer = false;
        return INode.ENodeState.ENS_Failure;
    }

    private INode.ENodeState CheckDetectPlayer()
    {
        if (DetectPlayer)
        {
            return INode.ENodeState.ENS_Success;
        }
        else
        {
            return INode.ENodeState.ENS_Failure;
        }

    }
    bool DetectPlayer;
    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints;
        public List<Vector3> blockedPoints;
    }
    public bool GetPlayer()
    {
        return DetectPlayer;
    }
    public VisibilityResult CheckVisibility(int rayCount)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        Transform enemyTransform = transform;

        // ��ä�� ������ Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // ����

            // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
            rayDirection.y = 0;

            // Raycast ����
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
                if (hit.collider.GetComponent<Player>())
                {
                    DetectPlayer = true;
                    return result;
                }
                else
                {
                    DetectPlayer = false;
                }
                result.visiblePoints.Add(hit.point);

            }
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);
            }
        }

        return result;


    }




}
