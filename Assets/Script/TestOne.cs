using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestOne : MonoBehaviour
{

    [Range(2, 64)]
    public int segments = 16;
    public Color visibleColor = Color.red;
    public Color blockedColor = new Color(1f, 0f, 0f, 0.5f); // ������ ����

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = CreateFanMaterial();
    }

    void Update()
    {
        GuardAI enemyAI = GetComponentInParent<GuardAI>();
        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI script not found!");
            return;
        }

        if (!enemyAI.GetPlayer())
        {
            GuardAI.VisibilityResult visibility = enemyAI.CheckVisibility(segments);
            meshFilter.mesh = CreateFanMesh(visibility);
        }
        else
        {
            meshFilter.mesh.Clear();
        }
        //Debug.Log("Mesh Assigned: " + (meshFilter.mesh != null));
    }

    Material CreateFanMaterial()
    {
        // "Unlit/VertexColor" ���̴� ��� (���̴� ���� �ʼ�)
        Shader shader = Shader.Find("Custom/UnlitVertexColor");
        if (shader == null)
        {
            Debug.LogError("'Unlit/VertexColor' shader not found!");
            return null; // ���̴��� ã�� ���ϸ� null ��ȯ
        }
        Debug.Log("Fan Material Created with Shader: " + shader.name);
        return new Material(shader);
    }

    Mesh CreateFanMesh(GuardAI.VisibilityResult visibility)
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
            colors[blockedStartIndex + i] = Color.red; // Blocked �κ� ���� ����
        }

        // �ﰢ�� �ε��� ����
        List<int> triangles = new List<int>();

        // Visible �κ� �ﰢ��
        for (int i = 0; i < visiblePoints.Count - 1; i++)
        {
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

