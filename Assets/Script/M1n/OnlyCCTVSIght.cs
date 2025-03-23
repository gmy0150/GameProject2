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

        // ���� ����: �߽���(0) + visiblePoints + blockedPoints
        Vector3[] vertices = new Vector3[1 + visiblePoints.Count + blockedPoints.Count];
        Color[] colors = new Color[vertices.Length];  // ���� �迭 �߰�

        vertices[0] = Vector3.zero; // �߽����� �׻� ���� ��ǥ (0, 0, 0)
        colors[0] = Color.red;    // �߽��� ���� (�⺻��)

        // Visible Points (���� ��ǥ�� ��ȯ)
        for (int i = 0; i < visiblePoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(visiblePoints[i]);
            colors[i + 1] = Color.red; // Visible �κ� ���� ����

        }

        // Blocked Points (���� ��ǥ�� ��ȯ)
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

        // Blocked �κ� �ﰢ��
        for (int i = 0; i < blockedPoints.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(blockedStartIndex + i);
            triangles.Add(blockedStartIndex + i + 1);
        }

        // �޽� ����
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();
        mesh.colors = colors; // ���� ����
        mesh.RecalculateNormals();

        return mesh;
    }
}
