using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTwo : MonoBehaviour
{
    public TestOne fanRenderer; // FanRenderer ����

    private void Start()
    {
        fanRenderer = GetComponentInChildren<TestOne>();
        if (fanRenderer == null)
        {
            Debug.LogError("FanRenderer (TestOne) not found in children!");
        }
    }

    public struct VisibilityResult
    {
        public List<Vector3> visiblePoints;
        public List<Vector3> blockedPoints;
    }

    public VisibilityResult CheckVisibility(int rayCount)
    {
        VisibilityResult result = new VisibilityResult();
        result.visiblePoints = new List<Vector3>();
        result.blockedPoints = new List<Vector3>();

        Transform enemyTransform = transform;

        // ��ä�� ������ Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // ����

            // 2D ��鿡�� y���� �����ϰ� rayDirection�� y���� 0���� ����
            rayDirection.y = 0;

            // Raycast ����
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Player�� �����ϸ� visiblePoints�� �߰�
                if (hit.collider.GetComponent<Player>())
                {

                }
                result.visiblePoints.Add(hit.point);

            }
            else // Raycast�� �ƹ��Ϳ��� ���� ���� ��� (��ä�� ����)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);
            }
        }

        return result;
    }
}