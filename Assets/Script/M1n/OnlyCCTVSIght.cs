using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyCCTVSIght : TestOne
{
    public override Mesh CreateFanMesh(Enemy.VisibilityResult visibility)
    {
        Mesh mesh = new Mesh();
        List<Vector3> visiblePoints = visibility.visiblePoints;
        List<Vector3> blockedPoints = visibility.blockedPoints;

        // 정점 개수: 중심점(0) + visiblePoints + blockedPoints
        Vector3[] vertices = new Vector3[1 + visiblePoints.Count + blockedPoints.Count];
        Color[] colors = new Color[vertices.Length];  // 색상 배열 추가

        vertices[0] = Vector3.zero; // 중심점은 항상 로컬 좌표 (0, 0, 0)
        colors[0] = Color.red;    // 중심점 색상 (기본값)

        // Visible Points (로컬 좌표로 변환)
        for (int i = 0; i < visiblePoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(visiblePoints[i]);
            colors[i + 1] = Color.red; // Visible 부분 색상 설정

        }

        // Blocked Points (로컬 좌표로 변환)
        int blockedStartIndex = 1 + visiblePoints.Count;
        for (int i = 0; i < blockedPoints.Count; i++)
        {
            vertices[blockedStartIndex + i] = transform.InverseTransformPoint(blockedPoints[i]);
            colors[blockedStartIndex + i] = Color.red;
        }

        List<int> triangles = new List<int>();

        for (int i = 0; i < visiblePoints.Count - 1; i++)
        {
            Debug.Log(i);
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        // Blocked 부분 삼각형
        for (int i = 0; i < blockedPoints.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(blockedStartIndex + i);
            triangles.Add(blockedStartIndex + i + 1);
        }

        // 메시 설정
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors; // 색상 적용
        mesh.RecalculateNormals();

        return mesh;
    }
}
