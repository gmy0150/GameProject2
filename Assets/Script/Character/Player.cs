using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    IController controller;
    public float RunSpeed;
    public float CrouchSpeed;
    bool isCrouch,isRun;
    bool GenNoise;
    public GameObject Prefab;


    [Header("소리 거리")]
    public float RunNoise, WalkNoise, CoinNoise;
    float throwForce = 10f;
    float maxThrowDistance = 10;

    public override void Action()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        // 바닥에 레이캐스트, "Ground" 레이어에만 반응하도록 함
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            targetPoint = hit.point;
        }
        else
        {
            // 바닥에 맞지 않으면 최대 거리로 설정
            targetPoint = ray.origin + ray.direction * maxThrowDistance;
        }

        // 목표 지점과 y축 위치 고정
        targetPoint.y = transform.position.y;

        // 목표 지점과의 방향 벡터 계산
        Vector3 throwDirection = (targetPoint - transform.position).normalized;

        // 최대 거리 제한
        if (Vector3.Distance(transform.position, targetPoint) > maxThrowDistance)
        {
            targetPoint = transform.position + throwDirection * maxThrowDistance;
        }

        // 목표 지점으로 물체 생성
        GameObject projectile = Instantiate(Prefab, transform.position + throwDirection, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // AddForce로 물체 날리기
            rb.AddForce(throwDirection * throwForce + Vector3.up * 2.0f, ForceMode.Impulse);

            // 물체 떨어지도록 하기 (필요 시)
            CoinDrop(rb.gameObject);
        }

    }
    public void CoinDrop(GameObject prefab)
    {
        
    }
    

    // Start is called before the first frame update
    void Start()
    {

        IController KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;
        applyspeed = MoveSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
        TryRun();
        TryCrouch();
        
        if(!isRun && !isCrouch)
        {
            MakeNoise(gameObject, WalkNoise, 10);
        }

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
        isRun = true;
        applyspeed = RunSpeed;
        MakeNoise(gameObject, RunNoise, 10);

    }
    void RunningCancel()
    {
        isRun = false;
        applyspeed = MoveSpeed;
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
    void CrouchCancel()
    {
        isCrouch = false;
        GenNoise = true;
        applyspeed = MoveSpeed;
    }
    void Crouch()
    {
        isCrouch = true;
        GenNoise = false;
        applyspeed = CrouchSpeed;
    }
    public void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Vector3 origin = obj.transform.position;
        origin.y = 1.5f;

        for (float anglestep = 0; anglestep < 360f; anglestep += stepsize)
        {
            float currentAngle = anglestep * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle));
            Debug.DrawRay(origin, direction * radius, Color.red, 5f); // 1초 동안 레이 표시

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
