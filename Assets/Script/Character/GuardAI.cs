using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class GuardAI : Enemy
{
    [Header("���")]
    public Vector3[] wayPoints;//���
    Vector3 curPosition;
    public int wayPointIndex = 0;
    [Header("�þ߹���, �Ÿ�")]
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // ��ä�� ����
    public float Distance = 5f;   // ��ä�� ������
    bool DetectPlayer;
    bool isUsingNav;
    public NavMeshAgent agent;
    [Header("�̰� �ٲٸ� �� �÷� ������ ����")]
    public Color GuardColor = Color.green;

    private void Start()
    {
        applyspeed = MoveSpeed;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        for (int i = 1; i < wayPoints.Length; i++)
        {

            wayPoints[i] = new Vector3(wayPoints[i].x, transform.position.y, wayPoints[i].z);

        }
    }

    private void Awake()
    {
        _BTRunner = new BehaviorTreeRunner(SettingBT());
    }
    void Update()
    {
        _BTRunner.Operate();
        if (agent != null)
        {
            agent.speed = applyspeed;
        }
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

        // ��ä�� ������ Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -RadiusAngle / 2 + RadiusAngle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // ����

            // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
            rayDirection.y = 0;

            // Raycast ����
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, Distance))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
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
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
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
        StoptNav();  // Navmesh ���߱�
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

                if (angleDifference <= 1f) // ȸ���� ���� �Ϸ�� ����
                {
                isPath = true; 
                    return INode.ENodeState.ENS_Success;  
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                return INode.ENodeState.ENS_Running;  // ȸ�� ��
            }
        }
        if (isPath)
            return INode.ENodeState.ENS_Success;

        return INode.ENodeState.ENS_Running;
    }
    private IEnumerator WaitPoint()
    {

        yield return new WaitForSeconds(0.2f);
        // ȸ���� �ð� ���� �׳� �������� ȸ��
        float randomAngle = UnityEngine.Random.Range(-45, 45);

        // ���� ������ŭ ȸ���ϴ� Quaternion ����
        Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);

        // transform.rotation�� randomRotation�� �����Ͽ� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, randomRotation, 5 * Time.deltaTime);
        while (Quaternion.Angle(transform.rotation, randomRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, randomRotation, 5 * Time.deltaTime);
            yield return null; // ���� �����ӱ��� ���
        }

        // ȸ���� �Ϸ�Ǹ� ī��Ʈ ����
        currentRepeat++;
        // ȸ�� �� �ٷ� Success ��ȯ
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
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(curPosition, wayPoints[wayPointIndex], step);

            Vector3 direction = (wayPoints[wayPointIndex] - curPosition).normalized;
            if (Vector3.Distance(wayPoints[wayPointIndex], curPosition) < 0.1f)
            {
                wayPointIndex++;
                if (wayPointIndex == wayPoints.Length)
                {
                    wayPointIndex = 0;
                }
                direction = (wayPoints[wayPointIndex] - curPosition).normalized;
                currentLookDirection = direction;
                Quaternion lookRotation = Quaternion.LookRotation(currentLookDirection);

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5 * Time.deltaTime);
                isPath = false; isWaiting = false;
                currentRepeat = 0;
                return INode.ENodeState.ENS_Success;

            }
            else
            {
                lookAroundTimer += Time.deltaTime;
                if (lookAroundTimer >= lookAroundInterval)
                {
                    lookAroundTimer = 0f;

                    float randomAngle = UnityEngine.Random.Range(-lookAngle, lookAngle);
                    Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);
                    currentLookDirection = randomRotation * direction;
                }
            }
            if (currentLookDirection == Vector3.zero)
            {
                currentLookDirection = direction;
            }
            if (currentLookDirection != Vector3.zero) // ȸ���� �ʿ䰡 ���� ���� ����
            {
                Quaternion lookRotation = Quaternion.LookRotation(currentLookDirection);
                float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

                float rotationspeed = (angleDifference > 60f) ? 5f : 2f;

                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationspeed * Time.deltaTime);
            }

            // ��ǥ ������ �����ϸ� ���� ��������Ʈ�� �̵�


            return INode.ENodeState.ENS_Running; // ��θ� ���󰡴� ��
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
            Vector3 direction = (player.transform.position - transform.position).normalized;
            applyspeed = MoveSpeed * 2;
            transform.position += direction * applyspeed * Time.deltaTime;
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
            StoptNav();
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
        agent.SetDestination(GetNoise());
        StartNav();
        if (Vector3.Distance(GetNoise(), curPosition) < 0.1f)
        {

            StoptNav();
            return INode.ENodeState.ENS_Success;

        }
        return INode.ENodeState.ENS_Running;

    }

    public void StartNav()
    {
        isUsingNav = true;
        agent.isStopped = false;
    }
    public void StoptNav()
    {
        isUsingNav = false;
        agent.isStopped = true;
        InitNoise();
    }


}
