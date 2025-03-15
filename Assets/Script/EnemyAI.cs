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
    [Range(0, 360)]
    public float angle = 90f;  // 부채꼴 각도
    public float radius = 5f;   // 부채꼴 반지름
    [Range(2, 64)]
    public int segments = 16;   // 부채꼴 분할 수
    public Color fanColor = Color.gray; // 부채꼴 색상

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    void Start()
    {
        fanRenderer = transform.GetChild(0).GetComponent<TestOne>();
    }


    void Update()
    {
        
    }

    

    // 기즈모 그리기 (선택 사항)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);

        Vector3 left = Quaternion.Euler(0, -angle / 2, 0) * transform.forward;
        Vector3 right = Quaternion.Euler(0, angle / 2, 0) * transform.forward;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * radius);
        Gizmos.DrawLine(transform.position, transform.position + right * radius);
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

        if(distanceTOPlayer > radius)
            return INode.ENodeState.ENS_Failure;

        float angle = Vector3.Angle(enemyTrnas.forward, directionToPlayer);
        if ((angle > angle / 2f))
            return INode.ENodeState.ENS_Failure;

        RaycastHit hit;
        if (Physics.Raycast(enemyTrnas.position, directionToPlayer, out hit, radius))
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
    public TestOne fanRenderer; // FanRenderer 스크립트에 대한 참조

    // 시야 검사 결과를 저장할 구조체


    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints; // 보이는 지점 목록
        public List<Vector3> blockedPoints; // 막힌 지점 목록
    }


    // 시야 검사 함수
    public VisibilityResult CheckVisibility(int rayCount)
    {
        Transform enemyTransform = transform;
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        GameObject player = GameObject.FindGameObjectWithTag("Player"); // tag로 찾는게 안전.
        if (player == null)
        {
            return result; // 플레이어가 없으면 빈 결과 반환
        }


        // 부채꼴 내에서 여러 개의 Raycast를 쏨
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward;

            // 디버깅을 위해 Ray 그리기 (Scene 뷰에서 확인)
            Debug.DrawRay(enemyTransform.position, rayDirection * fanRenderer.radius, Color.yellow);


            RaycastHit hit;

            // Raycast 실행
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Raycast가 Player에 맞았으면 보이는 지점
                if (hit.collider.CompareTag("Player"))
                {
                    result.visiblePoints.Add(hit.point);
                    //Debug.Log("visible");
                }
                // 다른 물체에 맞았으면 막힌 지점
                else
                {
                    result.blockedPoints.Add(hit.point);
                    //Debug.Log("blocked");
                }
            }
            else // Ray가 아무것에도 맞지 않았다면, 최대 거리 지점을 사용 (부채꼴 경계)
            {

                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);

            }
        }

        return result; // 보이는/막힌 지점 목록 반환
    }
}
