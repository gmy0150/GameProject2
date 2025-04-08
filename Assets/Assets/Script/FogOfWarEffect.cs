using UnityEngine;

// public class FogOfWarEffect : MonoBehaviour
// {
//     public Material FogMaterial;
//     public Transform ViewTransform; // Player_test 위치
//     public int textureSize = 256; // 렌더 텍스처 크기
//     public float clearRadius = 5f; // 안개 제거 반경

//     private Player_test playerTest;
//     private RenderTexture fogTexture; // 렌더 텍스처

//     void Start()
//     {
//         if (ViewTransform != null)
//         {
//             playerTest = ViewTransform.GetComponent<Player_test>();
//             if (playerTest == null)
//             {
//                 Debug.LogError("FogOfWarEffect: ViewTransform does not have a Player_test component!");
//             }
//         }
//         else
//         {
//             Debug.LogError("FogOfWarEffect: ViewTransform is not assigned!");
//         }

//         // 렌더 텍스처 생성
//         fogTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGB32);
//         FogMaterial.SetTexture("_FogMask", fogTexture); // 셰이더에 텍스처 전달

//         // 렌더 텍스처 초기화 (전체 안개로 채우기)
//         ClearTexture(Color.black);
//     }

//     void Update()
//     {
//         if (FogMaterial == null || ViewTransform == null || playerTest == null) return;

//         FogMaterial.SetVector("_ViewPosition", ViewTransform.position);
//         FogMaterial.SetFloat("_ViewRadius", playerTest.detectionRange);
//     }

//     void OnDestroy()
//     {
//         if (fogTexture != null)
//         {
//             fogTexture.Release();
//         }
//     }

//     // 렌더 텍스처 전체 초기화
//     void ClearTexture(Color color)
//     {
//         RenderTexture.active = fogTexture;
//         GL.Clear(true, true, color);
//         RenderTexture.active = null;
//     }

//     // 특정 위치에 원형으로 안개 제거
//     void ClearCircle(Vector3 worldPosition, float radius, Color color)
//     {
//         // 월드 좌표를 렌더 텍스처 UV 좌표로 변환
//         Vector2 uv = WorldToUV(worldPosition);

//         // 렌더 텍스처에 그리기
//         DrawCircle(uv, radius / textureSize, color);
//     }

//     // 월드 좌표 -> 렌더 텍스처 UV 좌표 변환
//     Vector2 WorldToUV(Vector3 worldPosition)
//     {
//         // Player 기준 로컬 좌표로 변환
//         Vector3 localPosition = transform.InverseTransformPoint(worldPosition);

//         // UV 좌표 계산 (Plane 크기에 따라 조정 필요)
//         float uvX = localPosition.x + 0.5f;
//         float uvY = localPosition.z + 0.5f;

//         return new Vector2(uvX, uvY);
//     }

//     // 렌더 텍스처에 원 그리기
//     void DrawCircle(Vector2 center, float radius, Color color)
//     {
//         Material mat = new Material(Shader.Find("Unlit/Color"));
//         mat.SetPass(0);

//         GL.Begin(GL.TRIANGLES);

//         for (int i = 0; i < 360; i += 10)
//         {
//             float angle1 = i * Mathf.Deg2Rad;
//             float angle2 = (i + 10) * Mathf.Deg2Rad;

//             Vector2 p1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
//             Vector2 p2 = center + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;

//             GL.Color(color);
//             GL.Vertex3(center.x, center.y, 0);
//             GL.Vertex3(p1.x, p1.y, 0);
//             GL.Vertex3(p2.x, p2.y, 0);
//         }

//         GL.End();
//     }
// }
