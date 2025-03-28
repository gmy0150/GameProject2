using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : UseageInteract
{
    bool hasCoin = false;
    float maxThrowDistance;
    float maxThrowForce;
    LineRenderer lineRenderer;
    float gravity = -9.81f;
    GameObject CoinPrefab;
    public override void Interact(Player character, IController controller)
    {
        base.Interact(character, controller);
        Debug.Log("�ֿ�");
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
        CoinDrop();
        lineRenderer.positionCount = 0;
    }
    public void CoinDrop()
    {
        hasCoin = false;
    }
    public void GetCoin()
    {
        hasCoin = true;
    }
    public void HasCoin()
    {
        if (hasCoin) // ������ ����� ���� ������ ǥ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPoint;

            // ���콺 ��ġ�� ���� �浹�ϴ� ������ ��ǥ �������� ����
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.origin + ray.direction * maxThrowDistance;
            }

            targetPoint.y = character.transform.position.y; // ����鿡���� ��ǥ ����

            // ������ ���� ���
            Vector3 throwDirection = (targetPoint - character.transform.position).normalized;
            float distance = Vector3.Distance(character.transform.position, targetPoint);

            if (distance > maxThrowDistance)
            {
                targetPoint = character.transform.position + throwDirection * maxThrowDistance;
                distance = maxThrowDistance; // �ִ� �Ÿ��� ����
            }

            // ��ǥ ���������� �Ÿ��� ������ �� ���
            float throwForce = Mathf.Lerp(2f, maxThrowForce, distance / maxThrowDistance);

            // ������ ��θ� �ǽð����� �׸���
            DrawThrowPreview(throwDirection, throwForce);

            // ��Ŭ���ϸ� ���� ������
            if (Input.GetMouseButtonDown(0))
            {
                ThrowCoin(throwDirection, throwForce);
            }
        }
        else
        {
            lineRenderer.positionCount = 0; // ������ ������ ������ ����
        }
    }


    // ������ ��θ� �׸��� �Լ�
    void DrawThrowPreview(Vector3 throwDirection, float throwForce)
    {
        lineRenderer.positionCount = 0;

        Vector3 startPos = character.transform.position;
        Vector3 velocity = throwDirection * throwForce;
        velocity.y = throwForce * 0.2f; //  ���� ��İ� �ϰ��ǵ��� Y�� �̵��� ����

        int numSteps = 8;
        float timeStep = 0.1f;
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < numSteps; i++)
        {
            float time = i * timeStep;
            Vector3 position = startPos + velocity * time;
            position.y += gravity * time * time / 2f;

            if (position.y < 0) break;  // y ���� 0 �����̸� �׸��� ����

            positions.Add(position);
        }

        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    // ���� ������ �Լ�
    void ThrowCoin(Vector3 throwDirection, float throwForce)
    {
        Vector3 transpo = character.transform.position;
        transpo.y = character.transform.position.y + 1;
        GameObject coin = Instantiate(CoinPrefab, transpo + throwDirection, Quaternion.identity);
        Rigidbody rb = coin.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 force = throwDirection * throwForce;
            force.y = throwForce * 0.2f; //  Y�� �̵� ����
            rb.AddForce(force, ForceMode.Impulse);
        }

        InteractAgain();
    }
}
