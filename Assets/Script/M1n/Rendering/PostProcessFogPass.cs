using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessFogPass : ScriptableRenderPass
{
    private Material _fogMaterial;
    private RenderTargetHandle _temporaryColorTexture;

    // 셰이더 파라미터 캐싱
    private static readonly int ViewPositionID = Shader.PropertyToID("_ViewPosition");
    private static readonly int CircleRangeID = Shader.PropertyToID("_CircleRange");
    private static readonly int DetectionRangeID = Shader.PropertyToID("_DetectionRange");
    private static readonly int AngleLimitID = Shader.PropertyToID("_AngleLimit");
    private static readonly int ViewDirectionID = Shader.PropertyToID("_ViewDirection");
    private static readonly int MainTexID = Shader.PropertyToID("_MainTex"); // _MainTex ID 추가

    // 플레이어 정보 저장을 위한 필드
    private Vector3 _playerPosition;
    private Vector3 _playerDirection;
    private float _playerCircleRange;
    private float _playerDetectionRange;
    private float _playerAngleLimit;

    // 생성자 수정: RenderPassEvent와 Material을 직접 받음
    public PostProcessFogPass(RenderPassEvent renderPassEvent, Material fogMaterial)
    {
        // this._settings = settings; // 제거
        this.renderPassEvent = renderPassEvent; // 전달받은 이벤트 사용
        this._fogMaterial = fogMaterial; // 전달받은 머티리얼 사용
        _temporaryColorTexture.Init("_TemporaryColorTexture");
    }

    // 외부(Feature)에서 플레이어 정보를 받아 설정하는 메서드
    public void SetShaderParameters(Player player)
    {
        if (player != null)
        {
            _playerPosition = player.transform.position;
            _playerDirection = player.transform.forward;
            _playerCircleRange = player.CircleRange;
            _playerDetectionRange = player.detectionRange;
            _playerAngleLimit = player.angleLimit;
        }
        else
        {
             // 플레이어가 없으면 기본값 또는 안전한 값 설정
             _playerPosition = Vector3.zero;
             _playerDirection = Vector3.forward;
             _playerCircleRange = 0f;
             _playerDetectionRange = 0f;
             _playerAngleLimit = 0f;
             Debug.LogWarning("[PostProcessFogPass] Player object not found for setting shader parameters.");
        }
    }


    // 렌더 패스 실행 (실제 렌더링 로직)
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (_fogMaterial == null)
        {
            Debug.LogError("Fog Material이 할당되지 않았습니다.");
            return;
        }

        CameraData cameraData = renderingData.cameraData;
        if (cameraData.camera.cameraType != CameraType.Game) return;

        var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;
        CommandBuffer cmd = CommandBufferPool.Get("PostProcessFog");

        // 1. 현재 카메라 타겟을 임시 텍스처로 복사
        RenderTextureDescriptor opaqueDesc = cameraData.cameraTargetDescriptor;
        opaqueDesc.depthBufferBits = 0; // 깊이 버퍼는 필요 없음
        cmd.GetTemporaryRT(_temporaryColorTexture.id, opaqueDesc, FilterMode.Bilinear);
        Blit(cmd, cameraColorTarget, _temporaryColorTexture.Identifier()); // _cameraColorTarget -> cameraColorTarget

        // 2. 머티리얼에 최신 플레이어 정보 설정
        _fogMaterial.SetVector(ViewPositionID, _playerPosition);
        _fogMaterial.SetFloat(CircleRangeID, _playerCircleRange);
        _fogMaterial.SetFloat(DetectionRangeID, _playerDetectionRange);
        _fogMaterial.SetFloat(AngleLimitID, _playerAngleLimit);
        _fogMaterial.SetVector(ViewDirectionID, _playerDirection);

        // 3. 임시 텍스처를 입력(_MainTex)으로 사용하여 안개 효과 적용 후 카메라 타겟에 출력
        // Blit 내부에서 _MainTex를 사용하므로 SetGlobalTexture 대신 Blit 사용
        Blit(cmd, _temporaryColorTexture.Identifier(), cameraColorTarget, _fogMaterial);

        // 4. 임시 텍스처 해제
        cmd.ReleaseTemporaryRT(_temporaryColorTexture.id);

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
