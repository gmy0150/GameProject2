
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Character
{
    IController controller;
    public float RunSpeed;
    public float CrouchSpeed;
    bool isCrouch;
    bool GenNoise;
    public GameObject Prefab;


    [Header("소리 거리")]
    public float RunNoise, WalkNoise, CoinNoise;
    float throwForce = 10f;
    float maxThrowDistance = 10;
    public bool isHide;
    public bool GetHide()
    {
        return isHide;
    }

    public override void Action()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxThrowDistance;
        }

        targetPoint.y = transform.position.y;

        Vector3 throwDirection = (targetPoint - transform.position).normalized;

        if (Vector3.Distance(transform.position, targetPoint) > maxThrowDistance)
        {
            targetPoint = transform.position + throwDirection * maxThrowDistance;
        }

        // 목표 지점으로 물체 생성
        GameObject projectile = Instantiate(Prefab, transform.position + throwDirection, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 lookDir = hit.point - transform.position;
            lookDir.y = 0;

            if (lookDir.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = targetRotation;
            }
            rb.AddForce(throwDirection * throwForce + Vector3.up * 2.0f, ForceMode.Impulse);

        }

    }
    public LayerMask closetLayer;
    Collider nearCloset = null;
    bool Closet;
    bool Box;
    public void HideOnCloset()
    {
        if (nearCloset != null && Input.GetKeyDown(KeyCode.E)&&!Box)
        {
            {
                Hide();
            }
        }
    }

    void Hide()
    {

        isHide = !isHide;
        Closet = !Closet;
        if (isHide)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            GetComponentInChildren<Renderer>().enabled = false;
            Crouch();
            GenNoise = false;
            GetComponentInChildren<Collider>().enabled = false ;
            GetComponent<Rigidbody>().useGravity = false;
            controller = null;
            Debug.Log("옷장에 숨");
        }
        else
        {
            CrouchCancel();
            GetComponentInChildren<Renderer>().enabled = true;
            GetComponentInChildren<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            controller = KeyboardControll;
            GenNoise = true;
            Debug.Log("옷장에 나옴");
        }
    }
    float cooldownTime = 5;
    float lastTransTime = 0;

    bool firstTime = false;
    void TransBox()
    {
        if (Input.GetKeyDown(KeyCode.R) && !Closet && Box)
        {
            CancelTransformation();
        }
        else if(Input.GetKeyDown(KeyCode.R) && !Closet && Time.time - lastTransTime >= cooldownTime || Input.GetKeyDown(KeyCode.R) && !firstTime)
        {
            firstTime = true;
            Box = true;
            isHide=true;
            Crouch();
            Debug.Log("변신");
            applyspeed = MoveSpeed;
            GetComponent<MeshRenderer>().material.color = Color.green;

        }
        else if (Input.GetKeyDown(KeyCode.R)&& Time.time - lastTransTime < cooldownTime)
        {
            // 쿨타임 중일 때 변신을 시도할 경우
            //Debug.Log("쿨타임 중입니다. " + (cooldownTime - (Time.time - lastTransTime)) + "초 남았습니다.");
        }
        if (Box)
        {
            TransTimer += Time.deltaTime;
            if(TransTimer > 10)
            {
                CancelTransformation();

            }
        }
    }
    void CancelTransformation()
    {
        Box = false;
        isHide = false;
        CrouchCancel();
        Debug.Log("시간초풀림");
        lastTransTime = Time.time;
        GetComponent<MeshRenderer>().material.color = Color.red;
        TransTimer = 0;
    }
    float TransTimer;
    
    void DetectCloset()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4, closetLayer);
        nearCloset = colliders.Length > 0 ? colliders[0] : null;
    }

    IController KeyboardControll;
    void Start()
    {

        KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;
        applyspeed = MoveSpeed;
        applyNoise = WalkNoise;

        GenNoise = true;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
        if (!isHide)
        {
            TryRun();
            TryCrouch();
        }
        DetectCloset();
        HideOnCloset();
        TransBox();
        DetectEnemy();
    }

    public void TransSpeed(float speed)
    {
        applyspeed = speed;
    }
    public override float ReturnSpeed()
    {
        return applyspeed;
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    void Running()
    {
        if (isCrouch)
            Crouch();

        GenNoise = true;
        applyspeed = RunSpeed;
        applyNoise = RunNoise;


    }
    public override float ReturnNoise()
    {
        return applyNoise;
    }
    void RunningCancel()
    {

        applyspeed = MoveSpeed;
        applyNoise = WalkNoise;
        GenNoise = true;
    }
    void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            CrouchCancel();
        }
    }
    public override bool GetNoise()
    {
        return GenNoise;
    }
    void CrouchCancel()
    {
        isCrouch = false;
        GenNoise = true;
        applyspeed = MoveSpeed;
        applyNoise = WalkNoise;
    }
    void Crouch()
    {
        isCrouch = true;
        GenNoise = false;
        applyspeed = CrouchSpeed;
        applyNoise = 0;
    }
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, detectionMask);

        foreach (var collider in colliders)
        {
            // 콜라이더가 적의 부모 오브젝트인지 확인
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                // 적을 보이게 하고 DetectEnemies에 추가
                Debug.Log("5m 범위 내 적 발견: " + enemy.name);
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
                            Debug.Log("시야 내 적 발견: " + enemy.name);
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
        // 시각화: 플레이어 주위 5 유닛 거리 내에서 감지 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);

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
    }


    public void ListenSound(GuardAI enemy)
    {
        enemy.ShowOutline();
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Debug.Log("확인");
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
                if (hit.collider.GetComponent<Enemy>())
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();
                    enemy.ProbArea(origin);
                }
            }
        }
    }
}
