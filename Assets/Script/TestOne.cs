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
    public Color blockedColor = new Color(1f, 0f, 0f, 0.5f); // 반투명 빨강

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
        // "Unlit/VertexColor" 쉐이더 사용 (쉐이더 수정 필수)
        Shader shader = Shader.Find("Custom/UnlitVertexColor");
        if (shader == null)
        {
            Debug.LogError("'Unlit/VertexColor' shader not found!");
            return null; // 쉐이더를 찾지 못하면 null 반환
        }
        Debug.Log("Fan Material Created with Shader: " + shader.name);
        return new Material(shader);
    }

    Mesh CreateFanMesh(GuardAI.VisibilityResult visibility)
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
            colors[blockedStartIndex + i] = Color.red; // Blocked 부분 색상 설정
        }

        // 삼각형 인덱스 설정
        List<int> triangles = new List<int>();

        // Visible 부분 삼각형
        for (int i = 0; i < visiblePoints.Count - 1; i++)
        {
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

