using EPOOutline;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


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


    private void Awake()
    {

    }
    TestNode TestNode;
    private void Start()
    {
        RestartPatrol();

        HideShape();
        applyspeed = MoveSpeed;
        aIPath = GetComponent<AIPath>();

        for (int i = 1; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);
        }
        TestNode = new TestSelector(new List<TestNode>()
        {
            new TestSequence(new List<TestNode>()
            {
                new CheckPlayerInSight(this),
                new ChasePlayer(this),
            }),
            new TestSequence(new List<TestNode>()
            {
                new CheckNoise(this),
                new MoveProbArea(this),
            }),
            new TestSequence(new List<TestNode>()
            {
                new Patrol(this),
                new WaitSecond(this),
            }),
        });

    }


    bool targetRotationSet;
    public void SetTarget()
    {
        targetRotationSet = false;
    }

    Quaternion targetRotation;
    float rotationSpeed = 2;
    public void SeeOther()
    {

        if (!targetRotationSet)
        {
            targetRotation = Quaternion.LookRotation(noise - transform.position);
            targetRotationSet = true;
        }

        // Slerp를 통해 현재 회전과 고정된 목표 회전 사이를 부드럽게 보간합니다.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    void Update()
    {
        aIPath.maxSpeed = applyspeed;

        MakeNoise(gameObject, 15, 10);
        if (Input.GetKeyDown(KeyCode.V))
        {

            SetNoise();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartMove();
        }


        TestNode.Evaluate();
    }
    public void InitNoise()
    {
        noise = Vector3.zero;
    }
    public void SetNoise()
    {
        Player player = GameObject.FindAnyObjectByType<Player>();
        noise = player.transform.position;
    }
    Coroutine timer;
    float timeDuration = 5f;
    public override void ShowOutline()
    {
        Outlinable.OutlineParameters.Enabled = true;
        if (timer != null)
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
        }
        HideOutline();
    }

    public override void HideOutline()
    {
        Outlinable.OutlineParameters.Enabled = false;
    }
    Seeker seeker;
    void MoveToTarget(Vector3 newTarget)
    {
        aIPath.enabled = true;
        aIPath.destination = newTarget;

        aIPath.isStopped = false;

        //aIPath.SearchPath();
    }

    public override void ShowShape()
    {
        base.ShowShape();
        HideOutline();
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
            //Debug.DrawRay(origin, direction * radius, Color.red, 5f);

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

    public override void Action()
    {

    }

    public override float ReturnSpeed()
    {
        return applyspeed;
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

    Vector3 noise;

    public override void ProbArea(Vector3 pos)
    {
        noise = pos;
        noise.y = transform.position.y;
    }
    public Vector3 GetNoiseVec()
    {
        return noise;
    }
    public override bool GetNoise()
    {
        if (noise == Vector3.zero) return false;
        else return true;
    }
    bool EndProb = false;
    public void End()
    {
        EndProb = true;
    }
    public void UnEnd()
    {
        EndProb = false;
    }
    public bool isEnd()
    {
        return EndProb;
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
    public void StartChase(Player player)
    {
        MoveToTarget(player.transform.position);

        applyspeed = RunSpeed;
    }
    bool probSuccess = false;
    public void MoveProb(Vector3 vec)
    {
        MoveToTarget(vec);

        applyspeed = MoveSpeed;
        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(vec.x, curPos.y, vec.z);   
        float distanceToTarget = Vector3.Distance(transform.position, vec);
        Debug.Log(distanceToTarget);
        if (distanceToTarget < 0.5f)  // 원하는 도달 범위 설정
        {
            Debug.Log("도착!");
            probSuccess = true;
        }
        //if (aIPath.reachedDestination)
        //{
        //    Debug.Log("도착못함?");
        //    probSuccess = true;
        //}
    }
    public void InitProb()
    {
        probSuccess = false;
        RestartPatrol();
        UnEnd();
    }
    public bool GetProb()
    {
        return probSuccess;
    }


    bool isPatrolling = false;
    public bool CheckAI()
    {
        return aIPath.isStopped;
    }

    public void StopMove()
    {
        aIPath.enabled = false;
        //aIPath.isStopped = true;
    }
    public void StartMove()
    {

        aIPath.isStopped = false;
        Debug.Log(aIPath.isStopped);
    }
    bool patrolSuccess = false;
    public void Patrols()
    {

        MoveToTarget(wayPoints[wayPointIndex]);

        isPatrolling = true;
        if (aIPath.reachedDestination)
        {
            wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;
            aIPath.isStopped = true;
            patrolSuccess = true;
        }
    }
    
    public void StopPatrol()
    {
        if (isPatrolling)
        {
            isPatrolling = false;
        }
    }
    public bool GetPatrol()
    {
        return patrolSuccess;
    }

    public void RestartPatrol()
    {
        patrolSuccess = false;
    }
    public void missPlayer()
    {
        DetectPlayer = false;

    }
    public bool GetPlayer() => DetectPlayer;
















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
                if (hit.collider.GetComponentInChildren<Player>())
                {
                    DetectPlayer = true;
                    if (hit.collider.GetComponentInChildren<Player>().GetHide())
                    {
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
}
