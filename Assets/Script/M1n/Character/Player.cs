
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
    public float RunSound, WalkSound, CoinSound;
    public static float RunNoise, WalkNoise, CoinNoise;
    [Header("던지는 거리")]
    public float maxThrowDistance = 40;
    public float maxThrowForce = 40;
    InteractController interactController;
    public float interactionDistance = 5.0f;
    public bool ViewPoint;
    public bool InteractPoint;
    public Mesh BaseMesh;
    public Mesh BoxMesh;

    public LayerMask Layer;
    IController KeyboardControll;
    void Start()
    {
        RunNoise = RunSound;
        WalkNoise = WalkSound;
        CoinNoise = CoinSound;
        KeyboardControll = new KeyboardController();
        KeyboardControll.OnPosessed(this);
        this.controller = KeyboardControll;
        interactController = new InteractController();
        interactController.OnPosessed(this);

    }
    void Update()
    {
        if (interactController != null)
        {
            interactController.TIck(Time.deltaTime);
        }
        if (controller != null)
        {
            controller.Tick(Time.deltaTime);
        }
        DetectEnemy();
    }

    public InteractController GetInterAct()
    {
        return interactController;
    }
    public override void Action()
    {

    }

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
    public IController GetKey()
    {
        return KeyboardControll;
    }


    // Update is called once per frame
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
        Collider[] colliders = Physics.OverlapSphere(transform.position, CircleRange, detectionMask);

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



        // 시야 내 적 감지
        Collider[] frontColliders = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

        foreach (var collider in frontColliders)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Vector3 directionToEnemy = enemy.transform.position - transform.position;
                directionToEnemy.y = 1f;
                Vector3 forward = transform.forward;
                float angle = Vector3.Angle(forward, directionToEnemy);

                if (angle <= angleLimit) 
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, directionToEnemy, out hit, detectionRange, ~wallLayer))
                    {
                        Debug.DrawRay(transform.position, directionToEnemy.normalized * detectionRange, Color.red, 1f);

                        Debug.Log(hit.collider.name);
                        enemy.ShowShape();
                        DetectEnemies.Add(enemy);
                        
                    }
                }
            }
        }
    }
    // 플레이어 앞 방향으로 90도 시야 내에서 8 유닛 거리로 적 감지
    [Header("시야")]
    public float angleLimit = 60;
    public float detectionRange = 20;
    public float CircleRange = 8;
    // Gizmos로 시각화 (옵션)
    private void OnDrawGizmos()
    {
        if (ViewPoint)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxThrowDistance);


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
        if (InteractPoint)
        {

            Gizmos.color = Color.red;

            Vector3 position = transform.position;
            Vector3 forward2 = transform.forward;
            float angle = 30f; // 60도 부채꼴 (30도 + 30도)

            Quaternion leftRotation = Quaternion.Euler(0, -angle, 0);
            Quaternion rightRotation = Quaternion.Euler(0, angle, 0);
            Vector3 leftDirection = leftRotation * forward2 * interactionDistance;
            Vector3 rightDirection = rightRotation * forward2 * interactionDistance;

            // 원형 감지 범위 (파란색)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(position, interactionDistance);

            // 부채꼴 감지 범위 (초록색)
            Gizmos.color = Color.green;
            Gizmos.DrawLine(position, position + forward2 * interactionDistance);
            Gizmos.DrawLine(position, position + leftDirection);
            Gizmos.DrawLine(position, position + rightDirection);
        }
    }


    public void ListenSound(GuardAI enemy)
    {
        enemy.ShowOutline();
    }

    public override void MakeNoise(GameObject obj, float radius, float stepsize)
    {
        Vector3 origin = obj.transform.position;
        origin.y = 1f;

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
