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


    [Header("�Ҹ� �Ÿ�")]
    public float RunNoise, WalkNoise, CoinNoise;
    float throwForce = 10f;
    float maxThrowDistance = 10;

    public override void Action()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;

        // �ٴڿ� ����ĳ��Ʈ, "Ground" ���̾�� �����ϵ��� ��
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            targetPoint = hit.point;
        }
        else
        {
            // �ٴڿ� ���� ������ �ִ� �Ÿ��� ����
            targetPoint = ray.origin + ray.direction * maxThrowDistance;
        }

        // ��ǥ ������ y�� ��ġ ����
        targetPoint.y = transform.position.y;

        // ��ǥ �������� ���� ���� ���
        Vector3 throwDirection = (targetPoint - transform.position).normalized;

        // �ִ� �Ÿ� ����
        if (Vector3.Distance(transform.position, targetPoint) > maxThrowDistance)
        {
            targetPoint = transform.position + throwDirection * maxThrowDistance;
        }

        // ��ǥ �������� ��ü ����
        GameObject projectile = Instantiate(Prefab, transform.position + throwDirection, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // AddForce�� ��ü ������
            rb.AddForce(throwDirection * throwForce + Vector3.up * 2.0f, ForceMode.Impulse);

            // ��ü ���������� �ϱ� (�ʿ� ��)
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
            Debug.DrawRay(origin, direction * radius, Color.red, 5f); // 1�� ���� ���� ǥ��

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
