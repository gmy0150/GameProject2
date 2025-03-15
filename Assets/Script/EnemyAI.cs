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
    public float RadiusAngle = 90f;  // 부채꼴 각도
    public float Distance = 5f;   // 부채꼴 반지름
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


    // 기즈모 그리기 (선택 사항)
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
            if (direction != Vector3.zero) // 회전할 필요가 있을 때만 실행
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 30 * Time.deltaTime);
            }

            // 목표 지점에 도달하면 다음 웨이포인트로 이동
            if (Vector3.Distance(wayPoints[wayPointIndex], curPosition) < 0.1f)
            {
                wayPointIndex++;
            }

            return INode.ENodeState.ENS_Running; // 경로를 따라가는 중
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

        // 부채꼴 내에서 Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // 방향

            // 2D 평면에서 y축을 무시하고 rayDirection의 y값을 0으로 설정
            rayDirection.y = 0;

            // Raycast 실행
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Player를 감지하면 visiblePoints에 추가
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
            else // Raycast가 아무것에도 맞지 않은 경우 (부채꼴 끝점)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);
            }
        }

        return result;


    }




}
