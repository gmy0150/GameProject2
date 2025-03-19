using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using EPOOutline;
using Unity.VisualScripting;
public class GuardAI : Enemy
{
    [Header("경로")]
    public Vector3[] wayPoints;//경로
    Vector3 curPosition;
    public int wayPointIndex = 0;
    [Header("시야범위, 거리")]
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // 부채꼴 각도
    public float Distance = 5f;   // 부채꼴 반지름
    bool DetectPlayer;
    [Header("이거 바꾸면 그 컬러 색으로 보임")]
    public Color GuardColor = Color.green;

    [Header("이거 넣으면 네모로 표시")]
    public bool Loop = false;
    AIPath aIPath;

    public Outlinable Outlinable;

    private void Start()
    {
        HideShape();
        applyspeed = MoveSpeed;
        aIPath = GetComponent<AIPath>();
        

        for (int i = 1; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
        }

    }

    Coroutine timer;
    float timeDuration = 5f;
    public override void ShowOutline()
    {
        Outlinable.OutlineParameters.Enabled = true;
        if(timer != null)
        {
            StopCoroutine(timer);
        }
        timer = StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < timeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        Debug.Log($"{gameObject.name} : t{elapsedTime}");
        }
        HideOutline();

    }

    public override void HideOutline()
    {
        Outlinable.OutlineParameters.Enabled = false;
    }

    void MoveToTarget(Vector3 newTarget)
    {
        aIPath.destination = newTarget;

        aIPath.isStopped = false;
        aIPath.SearchPath();
    }
    void StopMove()
    {
        aIPath.isStopped = true;
        InitNoise();
    }
    public override void ShowShape()
    {
        base.ShowShape();
    }
    public override void HideShape()
    {
        base.HideShape();
    }
    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Vector3 origin = obj.transform.position;
        origin.y = 1.5f;

        for (float anglestep = 0; anglestep < 360f; anglestep += stepsize)
        {
            float currentAngle = anglestep * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle));
            Debug.DrawRay(origin, direction * radius, Color.red, 5f);

            RaycastHit[] hits = Physics.RaycastAll(origin, direction, radius);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<Player>())
                {
                    Player player = hit.collider.GetComponent<Player>();
                    player.ListenSound(this);
                }
            }
        }
    }
    private void Awake()
    {
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }
    void Update()
    {
        _BTRunner.Operate();
        
        aIPath.maxSpeed = applyspeed;
        
        MakeNoise(gameObject, 15,10);
    }
    BehaviorTreeRunner _BTRunner = null;
    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override float ReturnSpeed()
    {
        return MoveSpeed;
    }

    private void OnDrawGizmos()
    {
        if (wayPoints == null || wayPoints.Length == 0)
            return;

        Gizmos.color = GuardColor; 

        Vector3 newTrans = transform.position;
        newTrans.y = 4;
        for (int i = 0; i < wayPoints.Length; i++)
        {
            
            Gizmos.DrawCube(newTrans, Vector3.one);

            Gizmos.DrawSphere(wayPoints[i], 0.5f);

            if (i < wayPoints.Length - 1)
            {
                Gizmos.DrawLine(wayPoints[i], wayPoints[i + 1]);
            }
        }
        if (Loop && wayPoints.Length > 1)
        {
            Gizmos.DrawLine(wayPoints[wayPoints.Length - 1], wayPoints[0]);
        }
    }





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

    public bool GetPlayer()
    {
        return DetectPlayer;
    }

    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints;
        public List<Vector3> blockedPoints;
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
            float currentAngle = -RadiusAngle / 2 + RadiusAngle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // 방향

            // 2D 평면에서 y축을 무시하고 rayDirection의 y값을 0으로 설정
            rayDirection.y = 0;

            // Raycast 실행
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, Distance))
            {
                // Player를 감지하면 visiblePoints에 추가
                if (hit.collider.GetComponent<Player>())
                {

                    DetectPlayer = true;
                    if (hit.collider.GetComponent<Player>().GetHide()) { 
                        DetectPlayer = false;
                    }
                }
                else
                {
                    DetectPlayer = false;
                }

                result.visiblePoints.Add(hit.point);

            }
            else // Raycast가 아무것에도 맞지 않은 경우 (부채꼴 끝점)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * Distance);
            }
        }
        return result;
    }



    Vector3 noise;

    public override void ProbArea(Vector3 pos)
    {
        noise = pos;
        noise.y = transform.position.y;
    }
    Vector3 GetNoise()
    {
        return noise;
    }







    /// <summary>
    /// BehaviorTree
    /// </summary>
    /// <returns></returns>
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
                new ActionNode(ListenNoise),
                new ActionNode(MoveProbArea),
                new ActionNode(WaitAtPoint),
            }),
            new SequenceNode(new List<INode>()
            {
                new ActionNode(WaitAtPoint),
                new ActionNode(FollowPath),
            })
        });
    }
    Vector3 currentLookDirection;
    float waitTime = 5;
    float waitTimer = 0;
    bool isWaiting = false;
    int repeatCount = 3;
    public int currentRepeat = 0;
    bool isPath = false;
    private INode.ENodeState WaitAtPoint()
    {
        StopMove();  // Navmesh 멈추기
        if (!isPath)
        {

            if (!isWaiting)
            {
                StartCoroutine(WaitPoint());
                isWaiting = true;
                return INode.ENodeState.ENS_Running;

            }
            if (currentRepeat >= repeatCount)
            {

                Vector3 direction = (wayPoints[wayPointIndex] - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);


                float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

                StopAllCoroutines();

                if (angleDifference <= 1f) // 회전이 거의 완료된 상태
                {
                isPath = true; 
                    return INode.ENodeState.ENS_Success;  
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                return INode.ENodeState.ENS_Running;  // 회전 중
            }
        }
        if (isPath)
            return INode.ENodeState.ENS_Success;

        return INode.ENodeState.ENS_Running;
    }
    private IEnumerator WaitPoint()
    {

        yield return new WaitForSeconds(0.2f);
        // 회전할 시간 없이 그냥 랜덤으로 회전
        float randomAngle = UnityEngine.Random.Range(-45, 45);

        // 랜덤 각도만큼 회전하는 Quaternion 생성
        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);

        // transform.rotation에 randomRotation만 적용하여 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, randomRotation, 5 * Time.deltaTime);
        while (Quaternion.Angle(transform.rotation, randomRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, randomRotation, 5 * Time.deltaTime);
            yield return null; // 다음 프레임까지 대기
        }

        // 회전이 완료되면 카운트 증가
        currentRepeat++;
        // 회전 후 바로 Success 반환
        yield return new WaitForSeconds(0.2f);
        isWaiting = false;

    }
    public void InitNoise()
    {
        noise = Vector3.zero;
    }
    private float lookAroundTimer = 0f;
    private float lookAroundInterval = 1.5f;
    float lookAngle = 30f;
    private INode.ENodeState FollowPath()
    {
        curPosition = transform.position;
        if (wayPointIndex < wayPoints.Length)
        {
            MoveToTarget(wayPoints[wayPointIndex]);

            if (aIPath.reachedDestination)
            {

                wayPointIndex++;
                if (wayPointIndex == wayPoints.Length)
                {
                    wayPointIndex = 0;
                }
                isPath = false; isWaiting = false;
                currentRepeat = 0;
                return INode.ENodeState.ENS_Success;
            }
            //else
            //{
            //    lookAroundTimer += Time.deltaTime;
            //    if (lookAroundTimer >= lookAroundInterval)
            //    {
            //        lookAroundTimer = 0f;

            //        // 랜덤 회전 각도 생성
            //        float randomAngle = UnityEngine.Random.Range(-lookAngle, lookAngle);
            //        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);

            //    }

            //}


            return INode.ENodeState.ENS_Running; // 경로를 따라가는 중
        }
        else
        {
            currentRepeat = 0;
            wayPointIndex = 0;
            isPath = false; isWaiting = false;
            return INode.ENodeState.ENS_Success;
        }
    }

    private INode.ENodeState ChasePlayer()
    {
        Player player = GameObject.FindAnyObjectByType<Player>();

        if (player == null) return INode.ENodeState.ENS_Failure;

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (player.GetHide())
        {
            applyspeed = MoveSpeed;

            DetectPlayer = false;
            return INode.ENodeState.ENS_Failure;
        }
        if (distanceToPlayer < (Distance * 20))
        {
            MoveToTarget(player.transform.position);
            applyspeed = MoveSpeed * 2;
            
            return INode.ENodeState.ENS_Running;
        }
        applyspeed = MoveSpeed;
        DetectPlayer = false;
        return INode.ENodeState.ENS_Failure;
    }

    private INode.ENodeState CheckDetectPlayer()
    {
        if (DetectPlayer)
        {
            StopMove();

            isPath = false; isWaiting = false;

            return INode.ENodeState.ENS_Success;
        }
        else
        {
            return INode.ENodeState.ENS_Failure;
        }

    }
    private INode.ENodeState ListenNoise()
    {
        if (GetNoise() != Vector3.zero)
        {
            StopAllCoroutines();
            isPath = false; isWaiting = false;

            return INode.ENodeState.ENS_Success;
        }
        else
        {
            return INode.ENodeState.ENS_Failure;
        }
    }
    private INode.ENodeState MoveProbArea()
    {
        isPath = false; isWaiting = false;
        curPosition = transform.position;

        MoveToTarget(GetNoise());


        //agent.SetDestination(GetNoise());
        //StartNav();
        if (Vector3.Distance(GetNoise(), curPosition) < 1f || aIPath.reachedDestination)
        {
            Debug.Log("test");
            StopMove();
            return INode.ENodeState.ENS_Success;

        }
        return INode.ENodeState.ENS_Running;

    }

   

}
