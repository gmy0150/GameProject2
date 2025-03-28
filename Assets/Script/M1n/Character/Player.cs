
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    IController controller;
    public float CrouchSpeed;
    public GameObject Prefab;

    public LineRenderer lineRenderer;
    [Header("소리 거리")]
    public float RunNoise, WalkNoise, CoinNoise;
    public float maxThrowDistance = 40;
    public float maxThrowForce = 40;

    InteractController interactController;
    public InteractController GetInterAct()
    {
        return interactController;
    }
    public override void Action()
    {

    }

    public LayerMask closetLayer;
    public LayerMask Coin;



    public IController GetControll()
    {
        return KeyboardControll;
    }
    public void ControllerDisable()
    {
        controller = null;
    }
    public void ControllerEnable()
    {
        controller = KeyboardControll;
    }
    //void Hide()
    //{
    //    isHide = !isHide;
    //    Closet = !Closet;
    //    if (isHide)
    //    {
    //        Rigidbody rigidbody = GetComponent<Rigidbody>();
    //        rigidbody.velocity = Vector3.zero;
    //        Crouch();
    //        Render(false);
    //        controller = null;
    //        Debug.Log("옷장에 숨");
    //    }
    //    else
    //    {
    //        CrouchCancel();
    //        Render(true);
    //        controller = KeyboardControll;
    //        Debug.Log("옷장에 나옴");
    //    }
    //}
    //void Render(bool x)
    //{
    //    GenNoise = x;
    //    GetComponentInChildren<Renderer>().enabled = x;
    //    GetComponentInChildren<Collider>().enabled = x;
    //    GetComponent<Rigidbody>().useGravity = x;
    //}

    public Mesh BaseMesh;
    public Mesh BoxMesh;
    //void TransBox()
    //{
    //    if (Input.GetKeyDown(KeyCode.R) && !Closet && Box)
    //    {
    //        CancelTransformation();
    //    }
    //    else if (Input.GetKeyDown(KeyCode.R) && !Closet && Time.time - lastTransTime >= cooldownTime || Input.GetKeyDown(KeyCode.R) && !firstTime)
    //    {
    //        firstTime = true;
    //        Box = true;
    //        isHide = true;
    //        Crouch();
    //        Debug.Log("변신");
    //        applyspeed = MoveSpeed;
    //        SkinnedMeshRenderer skined = GetComponentInChildren<SkinnedMeshRenderer>();
    //        skined.sharedMesh = BoxMesh;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.R) && Time.time - lastTransTime < cooldownTime)
    //    {
    //        // 쿨타임 중일 때 변신을 시도할 경우
    //        //Debug.Log("쿨타임 중입니다. " + (cooldownTime - (Time.time - lastTransTime)) + "초 남았습니다.");
    //    }
    //    if (Box)
    //    {
    //        TransTimer += Time.deltaTime;
    //        if (TransTimer > 10)
    //        {
    //            CancelTransformation();

    //        }
    //    }
    //}
    //void CancelTransformation()
    //{
    //    Box = false;
    //    isHide = false;
    //    CrouchCancel();
    //    lastTransTime = Time.time;
    //    SkinnedMeshRenderer skined = GetComponentInChildren<SkinnedMeshRenderer>();
    //    skined.sharedMesh = BaseMesh;
    //    TransTimer = 0;
    //}
    //float TransTimer;

    //void DetectCloset()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, 4, closetLayer);
    //    nearCloset = colliders.Length > 0 ? colliders[0] : null;
    //}
    public LayerMask Layer;
    IController KeyboardControll;

    void Start()
    {
        KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;
        interactController = new InteractController();
        interactController.OnPosessed(this);

    }
    public IController GetKey()
    {
        Debug.Log(KeyboardControll);
        return KeyboardControll;
    }


    // Update is called once per frame
    void Update()
    {
        if(interactController != null)
        {
            interactController.TIck(Time.deltaTime);
        }
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //GetCoin();
        }



        DetectEnemy();
        //DetectCoin();
        //HasCoin();
    }
    //    public bool isCoin()
    //    {
    //        return hasCoin;
    //    }
    //    bool hasCoin;

    //    float gravity = -9.81f;
    //    void DetectCoin()
    //    {
    //        Collider[] colliders = Physics.OverlapSphere(transform.position, 4, Coin);
    //        GameObject CoinObj = colliders.Length > 0 ? colliders[0].gameObject : null;
    //        if (CoinObj != null && Input.GetKeyDown(KeyCode.E))
    //        {
    //            CoinObj.SetActive(false);
    //            GetCoin();
    //        }
    //    }
    //    public void HasCoin()
    //    {
    //        if (hasCoin) // 코인을 얻었을 때만 포물선 표시
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;
    //            Vector3 targetPoint;

    //            // 마우스 위치가 땅과 충돌하는 지점을 목표 지점으로 설정
    //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
    //            {
    //                targetPoint = hit.point;
    //            }
    //            else
    //            {
    //                targetPoint = ray.origin + ray.direction * maxThrowDistance;
    //            }

    //            targetPoint.y = transform.position.y; // 수평면에서만 목표 설정

    //            // 던지는 방향 계산
    //            Vector3 throwDirection = (targetPoint - transform.position).normalized;
    //            float distance = Vector3.Distance(transform.position, targetPoint);

    //            if (distance > maxThrowDistance)
    //            {
    //                targetPoint = transform.position + throwDirection * maxThrowDistance;
    //                distance = maxThrowDistance; // 최대 거리로 제한
    //            }

    //            // 목표 지점까지의 거리로 던지는 힘 계산
    //            float throwForce = Mathf.Lerp(2f, maxThrowForce, distance / maxThrowDistance);

    //            // 포물선 경로를 실시간으로 그리기
    //            DrawThrowPreview(throwDirection, throwForce);

    //            // 좌클릭하면 코인 던지기
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                ThrowCoin(throwDirection, throwForce);
    //            }
    //        }
    //        else
    //        {
    //            lineRenderer.positionCount = 0; // 코인이 없으면 포물선 숨김
    //        }
    //    }


    //// 포물선 경로를 그리는 함수
    //void DrawThrowPreview(Vector3 throwDirection, float throwForce)
    //{
    //    lineRenderer.positionCount = 0;

    //    Vector3 startPos = transform.position;
    //    Vector3 velocity = throwDirection * throwForce;
    //    velocity.y = throwForce * 0.2f; //  기존 방식과 일관되도록 Y축 이동량 조정

    //    int numSteps = 8;
    //    float timeStep = 0.1f;
    //    List<Vector3> positions = new List<Vector3>();

    //    for (int i = 0; i < numSteps; i++)
    //    {
    //        float time = i * timeStep;
    //        Vector3 position = startPos + velocity * time;
    //        position.y += gravity * time * time / 2f;

    //        if (position.y < 0) break;  // y 값이 0 이하이면 그리기 종료

    //        positions.Add(position);
    //    }

    //    lineRenderer.positionCount = positions.Count;
    //    lineRenderer.SetPositions(positions.ToArray());
    //}

    //// 코인 던지는 함수
    //void ThrowCoin(Vector3 throwDirection, float throwForce)
    //{
    //        Vector3 transpo = transform.position;
    //        transpo.y = transform.position.y + 1;
    //    GameObject coin = Instantiate(Prefab, transpo + throwDirection, Quaternion.identity);
    //    Rigidbody rb = coin.GetComponent<Rigidbody>();

    //    if (rb != null)
    //    {
    //        Vector3 force = throwDirection * throwForce;
    //        force.y = throwForce * 0.2f; //  Y축 이동 적용
    //        rb.AddForce(force, ForceMode.Impulse);
    //    }

    //    hasCoin = false; // 코인을 던졌으므로 상태 변경
    //    lineRenderer.positionCount = 0; // 포물선 숨기기
    //}

    //// 코인을 얻었을 때 호출되는 함수
    //public void GetCoin()
    //{
    //    hasCoin = true;
    //}
    public List<Enemy> DetectEnemies = new List<Enemy>();
public LayerMask detectionMask;  // LayerMask를 public으로 설정하여 인스펙터에서 수정 가능하게 함
public LayerMask wallLayer;
void DetectEnemy()
{
    // 이전에 감지된 적을 숨김
    foreach (var enemy in DetectEnemies)
    {
        enemy.HideShape();
    }
    DetectEnemies.Clear();

    // 플레이어 주위 5 유닛 거리 내에서 모든 콜라이더를 감지 (벽 제외)
    Collider[] colliders = Physics.OverlapSphere(transform.position, 8f, detectionMask);

    foreach (var collider in colliders)
    {
        // 콜라이더가 적의 부모 오브젝트인지 확인
        Enemy enemy = collider.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            enemy.ShowShape();
            DetectEnemies.Add(enemy);
        }
    }

    // 플레이어 앞 방향으로 90도 시야 내에서 8 유닛 거리로 적 감지
    float angleLimit = 60f; // 90도 시야의 반으로 45도
    float detectionRange = 20f;

    // 시야 내 적 감지
    Collider[] frontColliders = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

    foreach (var collider in frontColliders)
    {
        // 콜라이더가 적의 부모 오브젝트인지 확인
        Enemy enemy = collider.GetComponentInParent<Enemy>();
        if (enemy != null)
        {
            // 플레이어와 적의 방향 벡터를 계산
            Vector3 directionToEnemy = enemy.transform.position - transform.position;
            directionToEnemy.y = 1.2f; // y값 무시 (수평 방향만 고려)

            // 플레이어의 앞 방향 벡터
            Vector3 forward = transform.forward;

            // 플레이어의 전방 90도 시야 내에 적이 있는지 확인
            float angle = Vector3.Angle(forward, directionToEnemy);

            if (angle <= angleLimit) // 45도 이하 각도에 있을 때만 감지
            {
                // 벽을 뚫고 적을 감지하지 않도록 Raycast로 확인
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToEnemy, out hit, detectionRange, ~wallLayer))
                {
                    if (hit.collider.GetComponentInParent<Enemy>() != null)
                    {
                        enemy.ShowShape();
                        DetectEnemies.Add(enemy);
                    }
                }
            }
        }
    }
}

// Gizmos로 시각화 (옵션)
private void OnDrawGizmos()
{

    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, maxThrowDistance);

    // 시야 범위 및 각도 시각화
    float angleLimit = 60f; // 45도
    float detectionRange = 20f;

    // 전방 시야 범위 그리기
    Vector3 forward = transform.forward;
    Gizmos.color = Color.green;
    Gizmos.DrawLine(transform.position, transform.position + forward * detectionRange);

    // 시야 각도 시각화
    Vector3 leftBound = Quaternion.Euler(0, -angleLimit, 0) * forward * detectionRange;
    Vector3 rightBound = Quaternion.Euler(0, angleLimit, 0) * forward * detectionRange;

    Gizmos.color = Color.blue;
    Gizmos.DrawLine(transform.position, transform.position + leftBound);
    Gizmos.DrawLine(transform.position, transform.position + rightBound);

    // 시야 각도 내 영역을 시각적으로 그리기
    Gizmos.color = new Color(0, 1, 1, 0.1f); // 반투명 Cyan 색
    Gizmos.DrawLine(transform.position, transform.position + leftBound);
    Gizmos.DrawLine(transform.position, transform.position + rightBound);

    Gizmos.color = Color.red;

    // 최대 던짐 거리만큼의 원 그리기 (플레이어의 위치에서)
    Gizmos.DrawWireSphere(transform.position, maxThrowDistance);
}


public void ListenSound(GuardAI enemy)
{
    enemy.ShowOutline();
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
            if (hit.collider.GetComponent<Enemy>())
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                enemy.ProbArea(origin);
            }
        }
    }
}
}
