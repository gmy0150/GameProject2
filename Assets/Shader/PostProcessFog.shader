Shader "Hidden/PostProcessFogURP"
{
    Properties
    {
        _MainTex ("Screen Texture", 2D) = "white" {} // Input from Blit
        _FogColor ("Fog Color", Color) = (0.1, 0.1, 0.1, 1) // 어두운 회색 안개
        _ViewPosition ("View Position (World)", Vector) = (0,0,0,0)
        _CircleRange ("Circle Range", Float) = 8.0
        _DetectionRange ("Detection Range", Float) = 20.0
        _AngleLimit ("Angle Limit", Float) = 60.0
        _ViewDirection ("View Direction (World)", Vector) = (0,0,1,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        ZWrite Off Cull Off // 포스트 프로세싱 기본 설정

        Pass
        {
            Name "PostProcessFogPass"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl" // 깊이 텍스처 사용

            // Properties
            sampler2D _MainTex; // Blit으로 전달되는 화면 텍스처
            half4 _FogColor;
            float4 _ViewPosition;
            float _CircleRange;
            float _DetectionRange;
            float _AngleLimit;
            float4 _ViewDirection;

            // Blit은 기본적으로 삼각형 하나를 화면 전체에 그림
            // 정점 셰이더는 클립 공간 좌표와 UV만 전달
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION; // 클립 공간 좌표
                float2 uv          : TEXCOORD0;   // 화면 UV
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                // 정점 위치를 클립 공간으로 변환
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                // UV 좌표 전달 (Blit은 보통 정점 ID로 UV 계산)
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 1. 현재 픽셀의 월드 좌표 복원
                float rawDepth = SampleSceneDepth(IN.uv); // 현재 UV의 깊이 값 샘플링
                // 픽셀의 클립 공간 좌표 계산 (XY는 UV, Z는 깊이, W는 1)
                float4 positionCS = float4(IN.uv * 2.0 - 1.0, rawDepth, 1.0);
                // NDC(Normalized Device Coordinates) 좌표계에서는 Y축 방향이 반대일 수 있음
                #if UNITY_UV_STARTS_AT_TOP
                positionCS.y = -positionCS.y;
                #endif
                // 클립 공간 좌표를 월드 공간 좌표로 변환
                float4 positionWS = mul(UNITY_MATRIX_I_VP, positionCS);
                positionWS /= positionWS.w;

                // 2. 깊이 비교 (Occlusion Check)
                float sceneLinearEyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
                // 현재 픽셀의 깊이 (카메라로부터의 거리) 계산 필요
                // 클립 공간 Z를 사용하여 계산 (vert에서 positionCS.w 또는 다른 방법 사용 가능)
                // 여기서는 간단히 월드 좌표 거리 사용 (정확도는 떨어질 수 있음)
                float pixelDistance = length(positionWS.xyz - _WorldSpaceCameraPos);
                // 더 정확한 방법: positionCS.w를 사용하거나, view space depth 사용
                float pixelLinearEyeDepth = LinearEyeDepth(positionCS.z, _ZBufferParams); // positionCS.z는 rawDepth와 같음

                // 픽셀이 씬 깊이보다 뒤에 있는지 확인 (오차 허용)
                bool isOccluded = pixelLinearEyeDepth > sceneLinearEyeDepth + 0.05; // 오차값 조정 필요

                // 3. 시야 계산 (가려지지 않은 픽셀 대상)
                float3 toPixel = positionWS.xyz - _ViewPosition.xyz;
                float distanceToView = length(toPixel);
                bool inCircleRange = distanceToView < _CircleRange;

                float3 viewDir = normalize(_ViewDirection.xyz);
                float3 pixelDir = normalize(toPixel);
                // acos는 비용이 높으므로 코사인 값으로 비교
                float angleCos = dot(viewDir, pixelDir);
                float limitCos = cos(_AngleLimit * 0.5 * (3.14159265 / 180.0)); // 각도를 라디안으로 변환 후 코사인 계산

                bool inViewCone = (distanceToView < _DetectionRange) && (angleCos > limitCos);

                // 4. 최종 색상 결정
                if (isOccluded || !(inCircleRange || inViewCone))
                {
                    // 가려졌거나 시야 밖에 있으면 안개 색상 반환
                    return _FogColor;
                }
                else
                {
                    // 시야 내에 있고 가려지지 않았으면 원래 화면 색상 반환
                    half4 originalColor = tex2D(_MainTex, IN.uv);
                    return originalColor;
                }
            }
            ENDHLSL
        }
    }
    Fallback Off
}
