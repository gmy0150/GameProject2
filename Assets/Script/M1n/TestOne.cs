using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TestOne : MonoBehaviour
{

    [Range(2, 64)]
    public int segments = 16;


    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material BaseMesh;
    public Material InvMeshes;


    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        BaseMesh = CreateFanMaterial();
    }
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();


    }

    void Update()
    {
        Enemy enemyAI = GetComponentInParent<Enemy>();
        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI script not found!");
            return;
        }
        if (!enemyAI.GetPlayer() )
        {
            if (enemyAI.GetType().ToString() == "CCTV")
            {
                Player player = GameObject.FindAnyObjectByType<Player>();
                Enemy.VisibilityResult visibility = enemyAI.CheckVisibility(segments,player.transform.localPosition.y);
                meshFilter.mesh = CreateFanMesh(visibility);
            }
            else
            {
                Debug.Log(enemyAI.GetHome());

                if (enemyAI.IsHome())
                {
                    Debug.Log("?>");
                    Enemy.VisibilityResult visibility = enemyAI.CheckVisibility(segments);
                    meshFilter.mesh = CreateFanMesh(visibility);
                }
            }
        }

    }

    Material CreateFanMaterial()
    {

        Shader shader = Shader.Find("Custom/UnlitVertexColor");
        if (shader == null)
        {
            Debug.Log("못찾?");
            return null; 
        }
        return new Material(shader);
    }
    void CheckShowMat()
    {
        if (show)
        {
            meshRenderer.material = BaseMesh;

        }
        else
        {
            meshRenderer.material = InvMeshes;
        }
    }
    public virtual Mesh CreateFanMesh(Enemy.VisibilityResult visibility)
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
    public virtual Mesh CreateFanMesh(Enemy.VisibilityResult visibility,float any)
    {
        Mesh mesh = new Mesh();
        List<Vector3> visiblePoints = visibility.visiblePoints;
        List<Vector3> blockedPoints = visibility.blockedPoints;

        // 정점 개수: 중심점(0) + visiblePoints + blockedPoints
        Vector3[] vertices = new Vector3[1 + visiblePoints.Count + blockedPoints.Count];
        Color[] colors = new Color[vertices.Length];  // 색상 배열 추가

        vertices[0] = new Vector3(0, any, 0); // 중심점은 항상 로컬 좌표 (0, 0, 0)
        colors[0] = Color.red;    // 중심점 색상 (기본값)

        // Visible Points (로컬 좌표로 변환)
        for (int i = 0; i < visiblePoints.Count; i++)
        {
            Vector3 adjustPoint = visiblePoints[i];
            adjustPoint.y = any;

            vertices[i + 1] = transform.InverseTransformPoint(visiblePoints[i]);
            colors[i + 1] = Color.red; // Visible 부분 색상 설정

        }

        // Blocked Points (로컬 좌표로 변환)
        int blockedStartIndex = 1 + visiblePoints.Count;
        for (int i = 0; i < blockedPoints.Count; i++)
        {
            Vector3 adjustPoint = blockedPoints[i];
            adjustPoint.y = any;
            vertices[blockedStartIndex + i] = transform.InverseTransformPoint(blockedPoints[i]);
            colors[blockedStartIndex + i] = Color.red;
        }

        List<int> triangles = new List<int>();

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
    bool show;
    public void ShowMesh()
    {
        show = true;
        CheckShowMat();
    }
    public void InvMeshRen()
    {
        show = false; 
        CheckShowMat();

    }


}

