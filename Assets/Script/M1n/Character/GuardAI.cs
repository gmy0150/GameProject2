using EPOOutline;

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

using BehaviorTree;
public class GuardAI : Enemy
{
    
    [Header("���")]
    public Vector3[] wayPoints;//���
    public int wayPointIndex = 0;

    
    [Header("�̰� �ٲٸ� �� �÷� ������ ����")]
    public Color GuardColor = Color.green;

    [Header("�̰� ������ �׸�� ǥ��")]
    public bool Loop = false;
    

    public Outlinable Outlinable;
    //public Node node;

    private void Awake()
    {

    }
    //Node GuardNode;
    protected override void Start()
    {
        base.Start();
        RestartPatrol();
        LayerMask ground = LayerMask.GetMask("Ground");
        RaycastHit hit;
        float groundy = 0;
        if(Physics.Raycast(transform.position,Vector3.down,out hit,Mathf.Infinity,ground)){
            
            groundy = hit.point.y;
        }

        

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Vector3(wayPoints[i].x, groundy, wayPoints[i].z);
        }


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

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    protected override void Update()
    {
        base.Update();
        
        MakeNoise(gameObject, 15, 10);
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

    public override void EndProbarea()
    {
        base.EndProbarea();
        SetTarget();
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



    public override void InitProb()
    {
        probSuccess = false;
        RestartPatrol();
        UnEndProbArea();
    }
    bool ProbMove =false;
    public override void StartProb(){
        ProbMove = true;
    }
    public override void ProbEnd(){
        ProbMove = false;
    }
    public override bool isProb(){
        return ProbMove;
    }


    bool isPatrolling = false;
    public bool CheckAI()
    {
        return aIPath.isStopped;
    }



    bool patrolSuccess = false;
    public override void Patrols()
    {
        MoveToTarget(wayPoints[wayPointIndex],Move,MoveSpeed);
        isPatrolling = true;
        if (aIPath.reachedDestination)
        {
            wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;
            aIPath.isStopped = true;
            patrolSuccess = true;
        }
    }
    
    public override void StopPatrol()
    {
        if (isPatrolling)
        {
            isPatrolling = false;
        }
    }
    public override bool GetPatrol()
    {
        return patrolSuccess;
    }

    public override void RestartPatrol()
    {
        patrolSuccess = false;
    }
}
