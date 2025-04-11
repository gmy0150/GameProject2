// using UnityEngine;
// using UnityEngine.Rendering;
// using UnityEngine.Rendering.Universal;

// public class PostProcessFogFeature : ScriptableRendererFeature
// {
//     [System.Serializable]
//     public class PassSettings
//     {
//         public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
//         [Tooltip("포스트 프로세싱에 사용할 머티리얼 (비워두면 기본 셰이더로 생성)")]
//         public Material fogMaterial;
//         [Tooltip("렌더러 기능 생성 시 적용될 초기 안개 색상")]
//         public Color initialFogColor = new Color(0.1f, 0.1f, 0.1f, 1f); // 초기 색상 설정 변수 추가
//     }

//     public PassSettings settings = new PassSettings();
//     private PostProcessFogPass _fogPass;
//     private Material _runtimeMaterial; // 런타임에 사용할 머티리얼 참조

//     // 렌더러 기능 생성 시 호출
//     public override void Create()
//     {
//         // 런타임 머티리얼 생성 또는 복사
//         if (settings.fogMaterial != null)
//         {
//             // Inspector에서 머티리얼이 할당된 경우, 복사하여 사용 (원본 보호)
//             _runtimeMaterial = new Material(settings.fogMaterial); // new Material() 사용
//             Debug.Log("[PostProcessFogFeature] Copied pre-assigned fogMaterial for runtime use.");
//         }
//         else
//         {
//             // Inspector에서 머티리얼이 할당되지 않은 경우, 셰이더로 새로 생성
//             Debug.Log("[PostProcessFogFeature] settings.fogMaterial is null, attempting to create runtimeMaterial from shader.");
//             Shader fogShader = Shader.Find("Hidden/PostProcessFogURP");
//             if (fogShader != null)
//             {
//                 Debug.Log("[PostProcessFogFeature] Found shader 'Hidden/PostProcessFogURP'.");
//                 _runtimeMaterial = CoreUtils.CreateEngineMaterial(fogShader);
//                 if (_runtimeMaterial != null)
//                 {
//                     Debug.Log("[PostProcessFogFeature] Successfully created runtimeMaterial.");
//                 }
//                 else
//                 {
//                     Debug.LogError("[PostProcessFogFeature] Failed to create runtimeMaterial from shader.");
//                     return; // 생성 실패 시 중단
//                 }
//             }
//             else
//             {
//                 Debug.LogError("[PostProcessFogFeature] Shader 'Hidden/PostProcessFogURP' not found. Check shader path and name.");
//                 return; // 셰이더 없으면 중단
//             }
//         }

//         // 생성/복사된 런타임 머티리얼에 초기 색상 적용
//         if (_runtimeMaterial != null)
//         {
//             _runtimeMaterial.SetColor("_FogColor", settings.initialFogColor);
//             Debug.Log($"[PostProcessFogFeature] Applied initial fog color to runtimeMaterial: {settings.initialFogColor}");

//             // 렌더 패스 인스턴스 생성 (이제 _runtimeMaterial을 전달)
//             _fogPass = new PostProcessFogPass(settings.renderPassEvent, _runtimeMaterial); // 생성자 변경 필요
//             Debug.Log("[PostProcessFogFeature] PostProcessFogPass instance created.");
//         }
//         else
//         {
//              Debug.LogError("[PostProcessFogFeature] Cannot create PostProcessFogPass because runtimeMaterial is null.");
//         }
//     }

//     // 각 카메라 렌더링 시 호출되어 렌더 패스를 렌더러에 추가
//     public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
//     {
//         // _runtimeMaterial과 _fogPass가 유효한지 확인
//         if (_fogPass == null)
//         {
//             Debug.LogWarning("[PostProcessFogFeature] AddRenderPasses skipped: _fogPass is null.");
//             return;
//         }
//         if (_runtimeMaterial == null) // settings.fogMaterial 대신 _runtimeMaterial 확인
//         {
//              Debug.LogWarning("[PostProcessFogFeature] AddRenderPasses skipped: _runtimeMaterial is null.");
//              return;
//         }

//         // 렌더 패스 실행 전에 플레이어 정보 설정
//         Player player = Object.FindObjectOfType<Player>(); // 씬에서 플레이어 찾기 (성능 고려 필요)
//         if (player != null)
//         {
//             _fogPass.SetShaderParameters(player);
//         }
//         else
//         {
//              // 플레이어를 못 찾았을 경우 기본값 설정 (SetShaderParameters 내부에서 처리)
//              _fogPass.SetShaderParameters(null);
//              Debug.LogWarning("[PostProcessFogFeature] Player object not found in the scene."); // LogWarningOnce -> LogWarning
//         }

//         // 렌더 패스를 렌더러에 추가
//         renderer.EnqueuePass(_fogPass);
//     }

//     // 렌더러 기능 정리 시 호출 (에디터 종료 등)
//     protected override void Dispose(bool disposing)
//     {
//         // 런타임에 생성/복사한 머티리얼만 해제
//         CoreUtils.Destroy(_runtimeMaterial);
//         _runtimeMaterial = null;
//         _fogPass = null; // 패스 참조도 초기화
//     }
// }
