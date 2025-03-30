using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviorTree;

public class Enemy : Character
{
    public Material invMesh;
    public Material BaseMesh;
    protected bool DetectPlayer;
    protected AIPath aIPath;
    [Header("시야범위, 거리")]
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // 부채꼴 각도
    public float Distance = 5f;   // 부채꼴 반지름

    public Node node;
    Node NewNode;
    float PlayerY;

    protected virtual void Start()
    {
        if (GetComponent<AIPath>() != null)
        {
            aIPath = GetComponent<AIPath>();
            applyspeed = MoveSpeed;

        }
        if (node != null)
        {
            NewNode = node.Clone();


            NewNode.SetRunner(this);
        }
        HideShape();
        Player player = GameObject.FindAnyObjectByType<Player>();
        if (player != null)
        {
            PlayerY = player.transform.position.y;
        }

    }
    protected virtual void Update()
    {
        if (aIPath != null)
        {
            aIPath.maxSpeed = applyspeed;
        }
        if (NewNode != null)
        {
            NewNode.Evaluate();
        }
    }
    public virtual bool GetPlayer() => DetectPlayer;
    
    public virtual void missPlayer()
    {
        DetectPlayer = false;
        Debug.Log(DetectPlayer);

    }
    public virtual void StopMove()
    {
        if (aIPath != null)
        {
            aIPath.enabled = false;
        }
        if(chase)
            chase = false;
        //aIPath.isStopped = true;
    }
    public override void Action()
    {
        throw new System.NotImplementedException();
    }




    public virtual void ShowOutline() { }
    public virtual void HideOutline() { }
    public virtual void ShowShape()
    {
        GetComponent<MeshRenderer>().material = BaseMesh;
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            if(chase == false)
                t1.ShowMesh();
        }
    }

    public virtual void HideShape()
    {

        GetComponent<MeshRenderer>().material = invMesh;
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
        }
    }
    bool chase;
    public virtual void StartChase(Player player)
    {
        applyspeed = RunSpeed;
        MoveToTarget(player.transform.position);
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
            chase = true;   
        }
    }
     void GoHome(Vector3 newTarget)
    {
        aIPath.enabled = true;

        aIPath.destination = newTarget;

        aIPath.isStopped = false;

        //aIPath.SearchPath();
    }
    protected virtual void MoveToTarget(Vector3 newTarget)
    {
        aIPath.enabled = true;
        
        aIPath.destination = newTarget;

        aIPath.isStopped = false;

        //aIPath.SearchPath();
    }

    protected Vector3 noise;

    public virtual void ProbArea(Vector3 pos)
    {
        noise = pos;
        noise.y = transform.position.y;
    }
    public virtual Vector3 GetNoiseVec()
    {
        return noise;
    }

    public virtual void InitNoise()
    {
        noise = Vector3.zero;
    }
    public virtual void InitProb()
    {
    }
    protected bool EndProb = false;

    public virtual void EndProbarea()
    {
        EndProb = true;
    }

    public virtual void UnEndProbArea()
    {
        EndProb = false;
    }
    public virtual bool isEndProb()
    {
        return EndProb;
    }

    protected bool probSuccess = false;
    protected bool HomeSuccess = false;
    Vector3 HomeSave;
    public  void MoveHome()
    {
        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(HomeSave.x, curPos.y, HomeSave.z);
        GoHome(targetPos);
        applyspeed = MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        TestOne t1;
        Debug.Log(targetPos);
        Debug.Log(transform.position);

        t1 = GetComponentInChildren<TestOne>();
        t1.InvMeshRen();
        Debug.Log(distanceToTarget);
        if (distanceToTarget < 1f)  // 원하는 도달 범위 설정
        {
            Debug.Log("도착!");
            HomeSuccess = true;
            t1.ShowMesh();
        }
    }
    public void SetHome(Vector3 vec)
    {
        HomeSave = vec;
        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(HomeSave.x, curPos.y, HomeSave.z);
        HomeSave = targetPos;
    }
    public bool IsHome()
    {
        return Vector3.Distance(HomeSave, transform.position) < 1.0f;

    }
    public void HomeArrive() { HomeSuccess = false; }

    public bool GetHome() { return HomeSuccess; }
    
    public void MoveProb(Vector3 vec)
    {
        MoveToTarget(vec);

        applyspeed = MoveSpeed;
        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(vec.x, curPos.y, vec.z);
        float distanceToTarget = Vector3.Distance(transform.position, vec);
        if (distanceToTarget < 0.5f)  // 원하는 도달 범위 설정
        {
            probSuccess = true;
        }
    }

    public bool GetProb(){ return probSuccess;}
    public virtual void Patrols(){ }
    public virtual void StopPatrol(){}
    public virtual void RestartPatrol(){ }
    public virtual bool GetPatrol(){ return false;}









    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints;
        public List<Vector3> blockedPoints;
    }
    public GameObject RayShoot;
    public VisibilityResult CheckVisibility(int rayCount)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        Vector3 NewVector = transform.position;
        NewVector.y = PlayerY;


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
            if (Physics.Raycast(NewVector, rayDirection, out hit, Distance))
            {
                // Player를 감지하면 visiblePoints에 추가
                if (hit.collider.GetComponentInParent<Player>())
                {
                    DetectPlayer = true;
                    if (hit.collider.GetComponentInParent<Player>().GetInterAct().GetHide())
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
    public VisibilityResult CheckVisibility(int rayCount,float newy)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        Transform enemyTransform = transform;

        Vector3 rayStartPos = enemyTransform.position;
        rayStartPos.y = newy;
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
            if (Physics.Raycast(rayStartPos, rayDirection, out hit, Distance))
            {
                // Player를 감지하면 visiblePoints에 추가
                if (hit.collider.GetComponentInParent<Player>())
                {
                    DetectPlayer = true;
                    if (hit.collider.GetComponentInParent<Player>().GetInterAct().GetHide())
                    {
                        DetectPlayer = false;
                    }
                }
                else
                {
                    DetectPlayer = false;
                }
                
                result.visiblePoints.Add(hit.point);
                Debug.DrawRay(rayStartPos, rayDirection * hit.distance, Color.red, 0.1f);

            }
            else // Raycast가 아무것에도 맞지 않은 경우 (부채꼴 끝점)
            {
                Vector3 endPoint = rayStartPos + rayDirection * Distance;
                result.visiblePoints.Add(rayStartPos + rayDirection * Distance);
                Debug.DrawRay(rayStartPos, rayDirection * Distance, Color.green, 0.1f);

            }
        }
        return result;
    }




    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        throw new System.NotImplementedException();
    }
}
