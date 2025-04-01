using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : UseageInteract
{

    float maxThrowDistance;
    float maxThrowForce;
    LineRenderer lineRenderer;
    float gravity = -9.81f;
    GameObject CoinPrefab;
    Mesh Base;
    MeshFilter filter;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        filter = GetComponent<MeshFilter>();
        Base = filter.mesh;
        filter.mesh = null;
        GetCoin();
        maxThrowDistance = character.maxThrowDistance;
        maxThrowForce = character.maxThrowForce;
        lineRenderer = character.lineRenderer;
        CoinPrefab = character.Prefab;


    }
    private void Update()
    {
        if (lineRenderer != null)
        {
            HasCoin();
        }
        
    }


    public override void InteractAgain()
    {
        if (!shoot)
        {
            filter.mesh = Base;
            //GameObject throwobj = Instantiate(CoinPrefab, transform.position, Quaternion.identity);

            gameObject.SetActive(false);
            UseCoin();

        }
        else
        {
            ShootCoin();
        }
    }
    public void UseCoin()
    {
        ShootCoin();
            shoot = false;

    }
    public void ShootCoin()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }
        hasCoin = false;
    }
    public void GetCoin()
    {
        hasCoin = true;
    }
    public void HasCoin()
    {
        if (hasCoin) // 코인을 얻었을 때만 포물선 표시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;
            Vector3 ChTrans = character.transform.position;
            float newY = 0.5f;
            ChTrans.y = newY;

            // 마우스 위치가 땅과 충돌하는 지점을 목표 지점으로 설정
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * maxThrowDistance;
            }

            targetPoint.y = ChTrans.y; // 수평면에서만 목표 설정

            // 던지는 방향 계산
            Vector3 throwDirection = (targetPoint - ChTrans).normalized;
            float distance = Vector3.Distance(ChTrans, targetPoint);

            if (distance > maxThrowDistance)
            {
                targetPoint = ChTrans + throwDirection * maxThrowDistance;
                distance = maxThrowDistance; // 최대 거리로 제한
            }

            // 목표 지점까지의 거리로 던지는 힘 계산
            float throwForce = Mathf.Lerp(2f, maxThrowForce, distance / maxThrowDistance);

            // 포물선 경로를 실시간으로 그리기
            DrawThrowPreview(throwDirection, throwForce);

            // 좌클릭하면 코인 던지기
            if (Input.GetMouseButtonDown(0))
            {
                ThrowCoin(throwDirection, throwForce);
            }
        }
        else
        {
            lineRenderer.positionCount = 0; // 코인이 없으면 포물선 숨김

        }
    }


    // 포물선 경로를 그리는 함수
    void DrawThrowPreview(Vector3 throwDirection, float throwForce)
    {
        lineRenderer.positionCount = 0;
        Vector3 ChTrans = character.transform.position;
        float newY = 1.5f;
        ChTrans.y = newY;
        Vector3 startPos = ChTrans;
        Vector3 velocity = throwDirection * throwForce;
        velocity.y = throwForce * 0.2f; //  기존 방식과 일관되도록 Y축 이동량 조정

        int numSteps = 20;
        float timeStep = 0.05f;
        List<Vector3> positions = new List<Vector3>();

        Vector3 lastPosition = startPos;


        for (int i = 0; i < numSteps; i++)
        {
            float time = i * timeStep;
            Vector3 position = startPos + velocity * time;
            position.y += gravity * time * time / 2f;
            if (Physics.Raycast(lastPosition, position - lastPosition, out RaycastHit hit ,(position - lastPosition).magnitude, LayerMask.GetMask("Ground")))
            {
                positions.Add(position);

                break;
            }
            positions.Add(position);

        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }


    void ThrowCoin(Vector3 throwDirection, float throwForce)
    {
        shoot = true;
        Vector3 transpo = character.transform.position;
        transpo.y = character.transform.position.y + 1;
        transform.position = transpo + throwDirection;
        //throwobj = Instantiate(CoinPrefab, transpo + throwDirection, Quaternion.identity);

        Rigidbody rb = transform.GetComponent<Rigidbody>();
            filter.mesh = Base;

        if (rb != null)
        {
            Vector3 force = throwDirection * throwForce;
            force.y = throwForce * 0.2f; //  Y축 이동 적용
            rb.AddForce(force, ForceMode.Impulse);
            character.GetInterActControll().ResetInteraction();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && shoot )
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.velocity = Vector3.zero;
            UseCoin();
            if (character != null)
            {
                character.MakeNoise(gameObject, Player.CoinNoise, 12);
            }
        }
    }
}
