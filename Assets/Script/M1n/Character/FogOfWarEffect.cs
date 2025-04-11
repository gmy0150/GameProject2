// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.Rendering; // CommandBuffer 사용

// [RequireComponent(typeof(MeshRenderer))] // 안개 평면 오브젝트에 붙어있어야 함
// public class FogOfWarEffect : MonoBehaviour
// {
//     [Header("References")]
//     [Tooltip("플레이어 오브젝트 참조")]
//     public Player player;
//     private Material fogMaterial; // 인스턴스화된 머티리얼

//     [Header("Visibility Calculation")]
//     [Tooltip("가시성 텍스처 해상도")]
//     public int textureSize = 128;
//     [Tooltip("플레이어 주변 가시성 체크 범위 (월드 유닛)")]
//     public float visibilityCheckRadius = 30f;
//     [Tooltip("가시성을 가로막는 장애물 레이어")]
//     public LayerMask obstacleLayerMask;
//     [Tooltip("레이캐스트 시작점 높이 오프셋")]
//     public float raycastOriginHeight = 0.5f;
//     [Tooltip("텍스처 업데이트 주기 (초, 0이면 매 프레임)")]
//     public float updateInterval = 0.1f;
//     [Tooltip("원형 시야 메시 분할 수")]
//     public int circleSegments = 32;
//     public float viewConeResolution = 0.5f;
//     [Tooltip("가시성 텍스처 원점을 맞출 그리드 크기 (0이면 사용 안 함)")]
//     public float gridSnapSize = 10f; // 그리드 스냅 크기 추가

//     // 내부 변수
//     private RenderTexture visibilityMaskTexture;
//     private float _lastUpdateTime = -1f;
//     private MeshRenderer _meshRenderer; // Awake에서 참조
//     private Mesh _visibilityMesh;
//     private Material _drawVisibilityMat;
//     private List<Vector3> _visibleVertices = new List<Vector3>(); // 가시성 메시 정점 리스트
//     private List<int> _visibleTriangles = new List<int>();   // 가시성 메시 삼각형 리스트

//     // 셰이더 프로퍼티 ID 캐싱
//     private static readonly int VisibilityMaskID = Shader.PropertyToID("_VisibilityMask");
//     private static readonly int VisibilityParamsID = Shader.PropertyToID("_VisibilityParams");
//     private static readonly int ViewPositionID = Shader.PropertyToID("_ViewPosition");
//     private static readonly int CircleRangeID = Shader.PropertyToID("_CircleRange");
//     private static readonly int ViewRadiusID = Shader.PropertyToID("_ViewRadius");
//     private static readonly int ViewAngleID = Shader.PropertyToID("_ViewAngle");
//     private static readonly int ViewDirectionID = Shader.PropertyToID("_ViewDirection");

//     void Awake() // Awake에서 컴포넌트 및 머티리얼 참조 가져오기
//     {
//         _meshRenderer = GetComponent<MeshRenderer>();
//         if (_meshRenderer != null && _meshRenderer.sharedMaterial != null)
//         {
//             // 머티리얼 인스턴스 생성 (Awake에서 미리)
//             fogMaterial = _meshRenderer.material;
//         }
//         else
//         {
//              Debug.LogError("FogOfWarEffect (Awake): MeshRenderer 또는 sharedMaterial이 없습니다!", this);
//              enabled = false;
//         }
//     }

//     void Start()
//     {
//         // Start에서는 참조가 유효한지만 최종 확인
//         if (!ValidateSetup()) return;

//         InitializeVisibilityTexture();
//         InitializeVisibilityDrawing(); // 가시성 그리기 관련 초기화

//         // 초기 머티리얼 설정
//         if (fogMaterial != null && visibilityMaskTexture != null)
//         {
//             fogMaterial.SetTexture(VisibilityMaskID, visibilityMaskTexture);
//         }
//     }

//     bool ValidateSetup()
//     {
//         if (player == null)
//         {
//             Debug.LogError("FogOfWarEffect: Player가 할당되지 않았습니다!", this);
//             enabled = false;
//             return false;
//         }
//         if (fogMaterial == null) // 인스턴스화 후 체크
//         {
//              Debug.LogError("FogOfWarEffect: Fog Material 인스턴스 생성 실패!", this);
//              enabled = false;
//              return false;
//         }
//         return true;
//     }

//     void InitializeVisibilityTexture()
//     {
//         if (visibilityMaskTexture != null && visibilityMaskTexture.IsCreated())
//         {
//             visibilityMaskTexture.Release();
//             DestroyImmediate(visibilityMaskTexture);
//         }

//         visibilityMaskTexture = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.R8);
//         visibilityMaskTexture.filterMode = FilterMode.Bilinear;
//         visibilityMaskTexture.wrapMode = TextureWrapMode.Clamp;
//         if (!visibilityMaskTexture.Create())
//         {
//             Debug.LogError("Failed to create visibility render texture!", this);
//             enabled = false;
//             return;
//         }

//         ClearTexture(Color.black); // ClearTexture 함수 복구
//         Debug.Log("[FogOfWarEffect] Visibility Texture Initialized.");
//     }

//     void InitializeVisibilityDrawing()
//     {
//         _visibilityMesh = new Mesh();
//         _visibilityMesh.name = "Visibility Mesh";
//         _visibilityMesh.MarkDynamic(); // 동적 업데이트 명시

//         Shader unlitShader = Shader.Find("Universal Render Pipeline/Unlit");
//         if (unlitShader == null) unlitShader = Shader.Find("Unlit/Color");
//         if (unlitShader != null)
//         {
//             _drawVisibilityMat = new Material(unlitShader);
//             // URP Unlit 셰이더는 _BaseColor 사용
//             if (_drawVisibilityMat.HasProperty("_BaseColor"))
//                  _drawVisibilityMat.SetColor("_BaseColor", Color.white);
//             else // 기본 Unlit/Color 셰이더는 _Color 사용
//                  _drawVisibilityMat.SetColor("_Color", Color.white);
//         }
//         else
//         {
//             Debug.LogError("Cannot find a suitable Unlit shader for drawing visibility!", this);
//         }
//     }

//     void Update() // 셰이더 파라미터 업데이트는 계속 Update 또는 LateUpdate에서 수행
//     {
//         if (player == null || fogMaterial == null || visibilityMaskTexture == null || !visibilityMaskTexture.IsCreated()) return;

//         // --- 플레이어 시야 파라미터 업데이트 (셰이더로 전달) ---
//         fogMaterial.SetVector(ViewPositionID, player.transform.position);
//         fogMaterial.SetFloat(CircleRangeID, player.CircleRange);
//         fogMaterial.SetFloat(ViewRadiusID, player.detectionRange);
//         fogMaterial.SetFloat(ViewAngleID, player.angleLimit);
//         fogMaterial.SetVector(ViewDirectionID, player.transform.forward);
//     }

//     void FixedUpdate() // 가시성 텍스처 업데이트는 FixedUpdate에서 수행
//     {
//         if (player == null || fogMaterial == null || visibilityMaskTexture == null || !visibilityMaskTexture.IsCreated()) return;

//         // updateInterval 제거하고 매 FixedUpdate마다 호출
//         UpdateVisibilityTexture();
//         // _lastUpdateTime = Time.time; // 더 이상 필요 없음
//     }

//     void UpdateVisibilityTexture()
//     {
//         Vector3 playerPos = player.transform.position;
//         Vector3 origin = playerPos + Vector3.up * raycastOriginHeight;

//         // 텍스처 영역 계산 (그리드 스냅 적용)
//         float halfSize = visibilityCheckRadius; // 텍스처 크기는 그대로 유지
//         float textureWorldSize = halfSize * 2f;
//         if (textureWorldSize <= 0) textureWorldSize = 1f;
//         float invTextureWorldSize = 1.0f / textureWorldSize;

//         // 텍스처 원점을 그리드에 맞춤
//         Vector2 textureOrigin;
//         if (gridSnapSize > 0)
//         {
//             textureOrigin = new Vector2(
//                 Mathf.Floor((playerPos.x - halfSize) / gridSnapSize) * gridSnapSize,
//                 Mathf.Floor((playerPos.z - halfSize) / gridSnapSize) * gridSnapSize
//             );
//         }
//         else // gridSnapSize가 0이면 플레이어 중심 사용 (기존 방식)
//         {
//              textureOrigin = new Vector2(playerPos.x - halfSize, playerPos.z - halfSize);
//         }

//         fogMaterial.SetVector(VisibilityParamsID, new Vector4(textureOrigin.x, textureOrigin.y, textureWorldSize, invTextureWorldSize));

//         CommandBuffer cmd = CommandBufferPool.Get("UpdateVisibilityMask");
//         cmd.SetRenderTarget(visibilityMaskTexture);
//         cmd.ClearRenderTarget(false, true, Color.black);

//         // 가시성 메시 생성
//         GenerateVisibilityMesh(origin, textureOrigin, invTextureWorldSize);

//         if (_drawVisibilityMat != null && _visibilityMesh.vertexCount > 0)
//         {
//             Matrix4x4 projectionMatrix = Matrix4x4.Ortho(0, 1, 0, 1, -1, 1);
//             cmd.SetViewProjectionMatrices(Matrix4x4.identity, projectionMatrix);

//             // 부채꼴 메시 그리기
//             cmd.DrawMesh(_visibilityMesh, Matrix4x4.identity, _drawVisibilityMat, 0, 0);

//             // 원형 시야 메시 생성 및 그리기
//             Vector2 playerCenterUV = (new Vector2(playerPos.x, playerPos.z) - textureOrigin) * invTextureWorldSize; // 변수 정의
//             float circleRadiusUV = player.CircleRange * invTextureWorldSize; // 변수 정의
//             Mesh circleMesh = CreateCircleMesh(playerCenterUV, circleRadiusUV, circleSegments); // 함수 호출 수정
//             if (circleMesh != null)
//             {
//                 cmd.DrawMesh(circleMesh, Matrix4x4.identity, _drawVisibilityMat, 0, 0);
//                 Object.DestroyImmediate(circleMesh);
//             }
//         }

//         Graphics.ExecuteCommandBuffer(cmd);
//         CommandBufferPool.Release(cmd);
//     }

//     void GenerateVisibilityMesh(Vector3 rayOrigin, Vector2 textureOrigin, float invTextureWorldSize)
//     {
//         _visibleVertices.Clear();
//         _visibleTriangles.Clear();

//         Vector2 playerCenterUV = (new Vector2(rayOrigin.x, rayOrigin.z) - textureOrigin) * invTextureWorldSize;
//         _visibleVertices.Add(new Vector3(playerCenterUV.x, playerCenterUV.y, 0));

//         int steps = Mathf.Max(3, Mathf.RoundToInt(player.angleLimit * viewConeResolution)); // 해상도 사용
//         if (steps <= 0) steps = 3; // 0 이하 방지
//         float angleStep = player.angleLimit / steps;
//         float startAngle = -player.angleLimit * 0.5f;

//         for (int i = 0; i <= steps; i++)
//         {
//             float currentAngle = startAngle + angleStep * i;
//             Vector3 direction = Quaternion.Euler(0, currentAngle, 0) * player.transform.forward;
//             direction.y = 0;

//             Vector3 hitPoint = rayOrigin + direction * player.detectionRange;

//             if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, player.detectionRange, obstacleLayerMask))
//             {
//                 hitPoint = hit.point;
//             }

//             Vector2 hitPointUV = (new Vector2(hitPoint.x, hitPoint.z) - textureOrigin) * invTextureWorldSize;
//             Vector3 currentVertexUV = new Vector3(hitPointUV.x, hitPointUV.y, 0);
//             _visibleVertices.Add(currentVertexUV);

//             if (i > 0)
//             {
//                 _visibleTriangles.Add(0);
//                 _visibleTriangles.Add(_visibleVertices.Count - 2);
//                 _visibleTriangles.Add(_visibleVertices.Count - 1);
//             }
//         }

//         _visibilityMesh.Clear();
//         // UV 좌표(0~1)를 그대로 정점 위치로 사용 (NDC 변환 제거)
//         _visibilityMesh.vertices = _visibleVertices.ToArray();
//         _visibilityMesh.triangles = _visibleTriangles.ToArray();
//         _visibilityMesh.RecalculateBounds();
//     }

//     // 원 메시 생성 함수 (클래스 내부로 이동)
//     Mesh CreateCircleMesh(Vector2 centerUV, float radiusUV, int segments)
//     {
//         Mesh mesh = new Mesh();
//         mesh.name = "Circle Visibility Mesh";
//         List<Vector3> vertices = new List<Vector3>();
//         List<int> triangles = new List<int>();

//         vertices.Add(new Vector3(centerUV.x, centerUV.y, 0)); // 중심점

//         float angleStep = 360f / segments;
//         for (int i = 0; i <= segments; i++)
//         {
//             float angle = i * angleStep * Mathf.Deg2Rad;
//             float x = centerUV.x + radiusUV * Mathf.Cos(angle);
//             float y = centerUV.y + radiusUV * Mathf.Sin(angle);
//             vertices.Add(new Vector3(x, y, 0));

//             if (i > 0)
//             {
//                 triangles.Add(0);
//                 triangles.Add(vertices.Count - 2);
//                 triangles.Add(vertices.Count - 1);
//             }
//         }

//         // UV 좌표(0~1)를 그대로 정점 위치로 사용 (NDC 변환 제거)
//         mesh.vertices = vertices.ToArray();
//         mesh.triangles = triangles.ToArray();
//         mesh.RecalculateBounds();
//         return mesh;
//     }

//     // ClearTexture 함수 (클래스 내부로 이동)
//     void ClearTexture(Color color)
//     {
//         RenderTexture rt = RenderTexture.active;
//         RenderTexture.active = visibilityMaskTexture;
//         GL.Clear(true, true, color);
//         RenderTexture.active = rt;
//     }

//     void OnDestroy()
//     {
//         if (visibilityMaskTexture != null && visibilityMaskTexture.IsCreated())
//         {
//              visibilityMaskTexture.Release();
//              DestroyImmediate(visibilityMaskTexture);
//         }
//         if (_visibilityMesh != null) Destroy(_visibilityMesh);
//         if (_drawVisibilityMat != null) Destroy(_drawVisibilityMat);
//         // fogMaterial은 MeshRenderer가 관리하므로 여기서 Destroy하지 않음
//     }
// }
