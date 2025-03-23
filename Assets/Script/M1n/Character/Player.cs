
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


    [Header("�Ҹ� �Ÿ�")]
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

        // ��ǥ �������� ��ü ����
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
            Debug.Log("���忡 ��");
        }
        else
        {
            CrouchCancel();
            GetComponentInChildren<Renderer>().enabled = true;
            GetComponentInChildren<Collider>().enabled = true;
            GetComponent<Rigidbody>().useGravity = true;
            controller = KeyboardControll;
            GenNoise = true;
            Debug.Log("���忡 ����");
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
            Debug.Log("����");
            applyspeed = MoveSpeed;
            GetComponent<MeshRenderer>().material.color = Color.green;

        }
        else if (Input.GetKeyDown(KeyCode.R)&& Time.time - lastTransTime < cooldownTime)
        {
            // ��Ÿ�� ���� �� ������ �õ��� ���
            //Debug.Log("��Ÿ�� ���Դϴ�. " + (cooldownTime - (Time.time - lastTransTime)) + "�� ���ҽ��ϴ�.");
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
        Debug.Log("�ð���Ǯ��");
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
    public LayerMask detectionMask;  // LayerMask�� public���� �����Ͽ� �ν����Ϳ��� ���� �����ϰ� ��
    public LayerMask wallLayer;
    void DetectEnemy()
    {
        // ������ ������ ���� ����
        foreach (var enemy in DetectEnemies)
        {
            enemy.HideShape();
        }
        DetectEnemies.Clear();

        // �÷��̾� ���� 5 ���� �Ÿ� ������ ��� �ݶ��̴��� ���� (�� ����)
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, detectionMask);

        foreach (var collider in colliders)
        {
            // �ݶ��̴��� ���� �θ� ������Ʈ���� Ȯ��
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                // ���� ���̰� �ϰ� DetectEnemies�� �߰�
                Debug.Log("5m ���� �� �� �߰�: " + enemy.name);
                enemy.ShowShape();
                DetectEnemies.Add(enemy);
            }
        }

        // �÷��̾� �� �������� 90�� �þ� ������ 8 ���� �Ÿ��� �� ����
        float angleLimit = 60f; // 90�� �þ��� ������ 45��
        float detectionRange = 20f;

        // �þ� �� �� ����
        Collider[] frontColliders = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

        foreach (var collider in frontColliders)
        {
            // �ݶ��̴��� ���� �θ� ������Ʈ���� Ȯ��
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                // �÷��̾�� ���� ���� ���͸� ���
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                directionToEnemy.y = 1.2f; // y�� ���� (���� ���⸸ ���)

                // �÷��̾��� �� ���� ����
                Vector3 forward = transform.forward;

                // �÷��̾��� ���� 90�� �þ� ���� ���� �ִ��� Ȯ��
                float angle = Vector3.Angle(forward, directionToEnemy);

                if (angle <= angleLimit) // 45�� ���� ������ ���� ���� ����
                {
                    // ���� �հ� ���� �������� �ʵ��� Raycast�� Ȯ��
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToEnemy, out hit, detectionRange, ~wallLayer))
                    {
                        if (hit.collider.GetComponentInParent<Enemy>() != null)
                        {
                            Debug.Log("�þ� �� �� �߰�: " + enemy.name);
                            enemy.ShowShape();
                            DetectEnemies.Add(enemy);
                        }
                    }
                }
            }
        }
    }

    // Gizmos�� �ð�ȭ (�ɼ�)
    private void OnDrawGizmos()
    {
        // �ð�ȭ: �÷��̾� ���� 5 ���� �Ÿ� ������ ���� ���� �ð�ȭ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 5f);

        // �þ� ���� �� ���� �ð�ȭ
        float angleLimit = 60f; // 45��
        float detectionRange = 20f;

        // ���� �þ� ���� �׸���
        Vector3 forward = transform.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + forward * detectionRange);

        // �þ� ���� �ð�ȭ
        Vector3 leftBound = Quaternion.Euler(0, -angleLimit, 0) * forward * detectionRange;
        Vector3 rightBound = Quaternion.Euler(0, angleLimit, 0) * forward * detectionRange;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBound);
        Gizmos.DrawLine(transform.position, transform.position + rightBound);

        // �þ� ���� �� ������ �ð������� �׸���
        Gizmos.color = new Color(0, 1, 1, 0.1f); // ������ Cyan ��
        Gizmos.DrawLine(transform.position, transform.position + leftBound);
        Gizmos.DrawLine(transform.position, transform.position + rightBound);
    }


    public void ListenSound(GuardAI enemy)
    {
        enemy.ShowOutline();
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Debug.Log("Ȯ��");
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
