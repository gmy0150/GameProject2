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
    public Material BaseMesh;
    public Material InvMeshes;


    void Start()
    {
       
    }
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter = GetComponent<MeshFilter>();
        BaseMesh = CreateFanMaterial();
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

                // if (enemyAI.IsHome() && enemyAI.GetType().ToString() == "GuardDog")
                // {
                //     Enemy.VisibilityResult visibility = enemyAI.CheckVisibility(segments);
                //     meshFilter.mesh = CreateFanMesh(visibility);
                // }
                
                if(enemyAI.GetType().ToString() == "GuardAI")
                {
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
            return null; 
        }
        Material mat = new Material(shader);
        mat.SetColor("_Color", Color.red);
    mat.SetFloat("_Alpha", 0.8f);
        return mat;
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

        // ���� ����: �߽���(0) + visiblePoints + blockedPoints
        Vector3[] vertices = new Vector3[1 + visiblePoints.Count + blockedPoints.Count];
        Color[] colors = new Color[vertices.Length];  // ���� �迭 �߰�

        vertices[0] = Vector3.zero; // �߽����� �׻� ���� ��ǥ (0, 0, 0)
        colors[0] = Color.red;    // �߽��� ���� (�⺻��)

        // Visible Points (���� ��ǥ�� ��ȯ)
        for (int i = 0; i < visiblePoints.Count; i++)
        {
            Vector3 localPos = transform.InverseTransformPoint(visiblePoints[i]);
            localPos.y = 0;
            vertices[i + 1] = localPos;
            colors[i + 1] = Color.red; // Visible �κ� ���� ����
            
        }

        // Blocked Points (���� ��ǥ�� ��ȯ)
        int blockedStartIndex = 1 + visiblePoints.Count;
        for (int i = 0; i < blockedPoints.Count; i++)
        {
            Vector3 localpos = transform.InverseTransformPoint(blockedPoints[i]);
            localpos.y = 0;
            vertices[blockedStartIndex + i] = localpos;
            colors[blockedStartIndex + i] = Color.red;
        }

        List<int> triangles = new List<int>();
        
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
    public virtual Mesh CreateFanMesh(Enemy.VisibilityResult visibility,float any)
    {
        Debug.Log("???");
        Mesh mesh = new Mesh();
        List<Vector3> visiblePoints = visibility.visiblePoints;
        List<Vector3> blockedPoints = visibility.blockedPoints;

        // ���� ����: �߽���(0) + visiblePoints + blockedPoints
        Vector3[] vertices = new Vector3[1 + visiblePoints.Count + blockedPoints.Count];
        Color[] colors = new Color[vertices.Length];  // ���� �迭 �߰�

        vertices[0] = new Vector3(0, any, 0); // �߽����� �׻� ���� ��ǥ (0, 0, 0)
        colors[0] = Color.red;    // �߽��� ���� (�⺻��)

        // Visible Points (���� ��ǥ�� ��ȯ)
        for (int i = 0; i < visiblePoints.Count; i++)
        {
            Vector3 adjustPoint = visiblePoints[i];
            adjustPoint.y = any;

            vertices[i + 1] = transform.InverseTransformPoint(visiblePoints[i]);
            colors[i + 1] = Color.red; // Visible �κ� ���� ����

        }

        // Blocked Points (���� ��ǥ�� ��ȯ)
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

