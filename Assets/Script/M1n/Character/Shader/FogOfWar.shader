Shader "Unlit/FogOfWarURP"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (Texture)", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)

        _FogColor ("Fog Color", Color) = (0.5,0.5,0.5,1)
        _ViewPosition ("View Position", Vector) = (0,0,0,0)
        _CircleRange ("Circle Range", Float) = 8.0 // Player.CircleRange
        _DetectionRange ("Detection Range", Float) = 20.0 // Player.detectionRange
        _AngleLimit ("Angle Limit", Float) = 60.0 // Player.angleLimit
        _ViewDirection ("View Direction", Vector) = (0,0,1,0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "IgnoreProjector"="True" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl" // 깊이 텍스처 선언 추가

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _FogColor;
                float4 _ViewPosition;
                float _CircleRange;
                float _DetectionRange; // 이름 변경: _ViewRadius -> _DetectionRange
                float _AngleLimit;     // 이름 변경: _ViewAngle -> _AngleLimit
                float4 _ViewDirection;
            CBUFFER_END

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION; // 클립 공간 좌표 (깊이 계산에 사용)
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1; // 월드 공간 좌표
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;

                // 깊이 비교 로직 추가
                float sceneRawDepth = SampleSceneDepth(IN.positionHCS.xy / IN.positionHCS.w); // 화면 좌표로 깊이 텍스처 샘플링
                float sceneLinearEyeDepth = LinearEyeDepth(sceneRawDepth, _ZBufferParams); // 선형 시점 공간 깊이로 변환
                float pixelLinearEyeDepth = LinearEyeDepth(IN.positionHCS.z, _ZBufferParams); // 현재 픽셀의 선형 시점 공간 깊이

                // 픽셀이 씬 깊이보다 뒤에 있는지 확인 (약간의 오차 허용)
                bool isOccluded = pixelLinearEyeDepth > sceneLinearEyeDepth + 0.01; // 0.01은 오차 보정값

                if (isOccluded)
                {
                    return _FogColor; // 가려졌으면 안개 색상 반환
                }

                // 기존 시야 계산 로직 (가려지지 않았을 때만 실행)
                float3 toPixel = IN.positionWS - _ViewPosition.xyz;
                float distanceToView = length(toPixel);

                // 1. 원형 시야 검사
                bool inCircleRange = distanceToView < _CircleRange;

                // 2. 전방 시야 검사
                float3 viewDir = normalize(_ViewDirection.xyz);
                float3 pixelDir = normalize(toPixel);
                float angleBetween = acos(dot(viewDir, pixelDir)) * (180.0 / 3.14159265359); // 라디안을 도로 변환

                bool inViewCone = (distanceToView < _DetectionRange) && (angleBetween < _AngleLimit); // 변수명 변경

                // 3. 최종 시야 결정 (가려지지 않은 경우)
                if (inCircleRange || inViewCone)
                {
                    return baseColor; // 시야 내에 있으면 기본 색상 반환
                }

                return _FogColor; // 시야 밖에 있으면 안개 색상 반환
            }
            ENDHLSL
        }
    }
}
