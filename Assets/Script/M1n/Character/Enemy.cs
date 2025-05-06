using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using BehaviorTree;

public class Enemy : Character
{
    protected bool DetectPlayer;
    protected AIPath aIPath;
    [Header("�þ߹���, �Ÿ�")]
    [Range(0, 360)]
    public float RadiusAngle = 90f;  // ��ä�� ����
    public float Distance = 5f;   // ��ä�� ������

    public Node node;
    Node NewNode;
    float PlayerY;
    public Animator anim;
    Collider collider;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
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
        ShowShape();
        Player player = GameObject.FindAnyObjectByType<Player>();
        if (player != null)
        {
            PlayerY = player.transform.position.y;
        }
        collider = GetComponentInChildren<Collider>();

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
        ShowShape();
    }
    bool stun;
    public void HitEnemy(){
        StopMove();
        missPlayer();
        stun = true;
        Time.timeScale = 0;
        //ui창 띄우기 애니매이션 재생
        Time.timeScale = 1;
    }
    public void releaseStun(){
        stun = false;
    }
    public bool GetStun(){
        return stun;
    }
    public virtual void StopMove()
    {
        if (aIPath != null)
        {
            UseAnim("Idle");
            aIPath.enabled = false;
        }
        if (chase)
            chase = false;
        //aIPath.isStopped = true;
    }
    string Idle = "Idle";
    string Move = "Move";
    string ChasePlayer = "ChasePlayer";
    string CheckNoise = "CheckNoise";
    public void UseAnim(string exclude){
        if(anim == null)return;
        
        string[] triggers = {Idle,Move,ChasePlayer,CheckNoise};
        foreach(string trigger in triggers){
            if(trigger != exclude){
                anim.SetBool(trigger,false);
            }else{
                anim.SetBool(trigger,true);
            }
        }
    }



    public virtual void ShowOutline() { }
    public virtual void HideOutline() { }
    public virtual void ShowShape()
    {
        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.ShowMesh();
        }
    }

    public virtual void HideShape()
    {

        if (GetComponentInChildren<TestOne>())
        {
            TestOne t1 = GetComponentInChildren<TestOne>();
            t1.InvMeshRen();
        }
    }
    bool chase;
    public virtual void StartChase(Player player)
    {
        if(stun)return;
        applyspeed = RunSpeed;
        UseAnim(ChasePlayer);
        aIPath.enabled = true;
        Vector3 newvec = player.transform.position;
        newvec.y = transform.position.y;
        aIPath.destination = newvec;
        aIPath.isStopped = false;
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
        if(stun)return;

        aIPath.enabled = true;
        newTarget.y = transform.position.y;
        aIPath.destination = newTarget;
        aIPath.isStopped = false;
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
        Debug.Log("왜 ");
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
    public void MoveHome()
    {
        Vector3 curPos = transform.position;
        Vector3 targetPos = new Vector3(HomeSave.x, curPos.y, HomeSave.z);
        GoHome(targetPos);
        applyspeed = MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        TestOne t1;

        t1 = GetComponentInChildren<TestOne>();
        t1.InvMeshRen();
        Debug.Log(distanceToTarget);
        if (distanceToTarget < 1f)  // ���ϴ� ���� ���� ����
        {
            Debug.Log("����!");
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
        if(stun)return;

        MoveToTarget(vec);

        applyspeed = MoveSpeed;
        Vector3 newVec = vec;
        newVec.y = transform.position.y;
        float distanceToTarget = Vector3.Distance(transform.position, newVec);
        if (distanceToTarget < 0.5f)  // ���ϴ� ���� ���� ����
        {
            probSuccess = true;
        }
    }
    public virtual void ProbEnd(){}
    public virtual void StartProb(){}
    public virtual bool isProb(){return false;}
    public bool GetProb() { return probSuccess; }
    public virtual void Patrols() { }
    public virtual void StopPatrol() { }
    public virtual void RestartPatrol() { }
    public virtual bool GetPatrol() { return false; }
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
        if(stun) return result;

        Vector3 NewVector = transform.position;
        NewVector.y = collider.bounds.center.y;


        Transform enemyTransform = transform;

        // ��ä�� ������ Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            if (DetectPlayer == true)
                return result;
                
            float currentAngle = -RadiusAngle / 2 + RadiusAngle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // ����

            // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
            rayDirection.y = 0;
            // Raycast ����
            RaycastHit hit;
            if (Physics.Raycast(NewVector, rayDirection, out hit, Distance))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
                if (hit.collider.GetComponentInParent<Player>())
                {
                    DetectPlayer = true;
                    // Debug.Log(hit.collider.GetComponent<Player>().GetInterActControll().GetHide());
                    if (hit.collider.GetComponentInParent<Player>().GetInterActControll().GetHide())
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
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * Distance);
            }
        }
        return result;
    }
    public VisibilityResult CheckVisibility(int rayCount, float newy)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();
        if(stun) return result;

        Transform enemyTransform = transform;

        Vector3 rayStartPos = enemyTransform.position;
        rayStartPos.y = newy;
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
            if (Physics.Raycast(rayStartPos, rayDirection, out hit, Distance))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
                if (hit.collider.GetComponentInParent<Player>())
                {
                    DetectPlayer = true;
                    if (hit.collider.GetComponentInParent<Player>().GetInterActControll().GetHide())
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
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
            {
                Vector3 endPoint = rayStartPos + rayDirection * Distance;
                result.visiblePoints.Add(rayStartPos + rayDirection * Distance);
                Debug.DrawRay(rayStartPos, rayDirection * Distance, Color.green, 0.1f);

            }
        }
        return result;
    }


    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy")){
            StartCoroutine(TempBlockENemy(collision.transform.position));
        }
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        throw new System.NotImplementedException();
    }
    IEnumerator TempBlockENemy(Vector3 enemyPos){
        Bounds bounds = new Bounds(enemyPos, Vector3.one * 0.5f);
        GraphUpdateObject guo = new GraphUpdateObject(bounds){
            modifyWalkability = true,setWalkability = false
        };
        AstarPath.active.UpdateGraphs(guo);
        yield return new WaitForSeconds(0.3f);
        GraphUpdateObject guorestore = new GraphUpdateObject(bounds){
            modifyWalkability = true,setWalkability = true
        };
        AstarPath.active.UpdateGraphs(guorestore);
    }
}
