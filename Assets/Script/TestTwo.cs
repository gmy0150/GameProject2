using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTwo : MonoBehaviour
{
    public TestOne fanRenderer; // FanRenderer 참조

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

        // 부채꼴 내에서 Raycast
        for (int i = 0; i <= rayCount; i++)
        {
            float currentAngle = -fanRenderer.angle / 2 + fanRenderer.angle * (i / (float)rayCount);
            Quaternion rotation = Quaternion.Euler(0, currentAngle, 0);
            Vector3 rayDirection = rotation * enemyTransform.forward; // 방향

            // 2D 평면에서 y축을 무시하고 rayDirection의 y값을 0으로 설정
            rayDirection.y = 0;

            // Raycast 실행
            RaycastHit hit;
            if (Physics.Raycast(enemyTransform.position, rayDirection, out hit, fanRenderer.radius))
            {
                // Player를 감지하면 visiblePoints에 추가
                if (hit.collider.GetComponent<Player>())
                {

                }
                result.visiblePoints.Add(hit.point);

            }
            else // Raycast가 아무것에도 맞지 않은 경우 (부채꼴 끝점)
            {
                result.visiblePoints.Add(enemyTransform.position + rayDirection * fanRenderer.radius);
            }
        }

        return result;
    }
}