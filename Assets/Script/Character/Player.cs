using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

public class Player : Character
{
    IController controller;
    public float RunSpeed;
    public float CrouchSpeed;
    bool isCrouch, isRun;
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
        Renderer renderer = GetComponent<Renderer>();

        isHide = !isHide;
        Closet = !Closet;
        if (isHide)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            if(renderer != null)            GetComponent<Renderer>().enabled = false;
            Crouch();
            GenNoise = false;
            GetComponent<Collider>().enabled = false ;
            GetComponent<Rigidbody>().useGravity = false;
            controller = null;
            Debug.Log("옷장에 숨");
        }
        else
        {
            CrouchCancel();
            if (renderer != null) GetComponent<Renderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
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
        GetComponent<MeshRenderer>().material.color = Color.gray;
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
        if (!isRun && !isCrouch && GenNoise)
        {
            MakeNoise(gameObject, WalkNoise, 10);
        }
        DetectCloset();
        HideOnCloset();
        TransBox();
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
        if(GenNoise)
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
