Shader "Unlit/FogOfWarURP"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (Texture)", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)

        _FogColor ("Fog Color", Color) = (0.5,0.5,0.5,1)
        _ViewPosition ("View Position", Vector) = (0,0,0,0)
        _CircleRange ("Circle Range", Float) = 8.0
        _ViewRadius ("View Radius", Float) = 20.0
        _ViewAngle ("View Angle", Float) = 60.0
        _ViewDirection ("View Direction", Vector) = (0,0,1,0)

        // 플레이어 기준 가시성 마스크 텍스처 추가
        _VisibilityMask ("Visibility Mask", 2D) = "white" {}
        // 가시성 텍스처 파라미터 (x=origin.x, y=origin.z, z=size, w=1/size)
        _VisibilityParams ("Visibility Params", Vector) = (0,0,50,0.02)
    }

    SubShader
    {
        // 투명 렌더링 설정 추가
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" "IgnoreProjector"="True" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            // 블렌딩 및 ZWrite 설정 (Pass 블록 안에 직접 위치)
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
                float _ViewRadius;
                float _ViewAngle;
                float4 _ViewDirection;
                // 가시성 관련 변수 추가
                float4 _VisibilityParams;
            CBUFFER_END

            TEXTURE2D(_BaseMap); SAMPLER(sampler_BaseMap);
            // 가시성 마스크 텍스처 샘플러 추가
            TEXTURE2D(_VisibilityMask); SAMPLER(sampler_VisibilityMask);

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
                // 1. 플레이어 기준 가시성 확인 복원
                float2 visibilityUV = (IN.positionWS.xz - _VisibilityParams.xy) * _VisibilityParams.w;
                half visibility = SAMPLE_TEXTURE2D(_VisibilityMask, sampler_VisibilityMask, visibilityUV).r;

                // smoothstep을 사용하여 가시성 경계를 부드럽게 처리
                // visibility 값이 0.4 ~ 0.6 사이에서 부드럽게 전환되도록 설정 (값 조절 가능)
                half smoothVisibility = smoothstep(0.4, 0.6, visibility);

                // 최종 색상 계산: 안개 색상과 투명 색상 사이를 보간
                // smoothVisibility가 0이면 안개색, 1이면 투명색
                half4 transparentColor = half4(0,0,0,0);
                half4 finalColor = lerp(_FogColor, transparentColor, smoothVisibility);

                return finalColor;


                /* // 시야 계산 로직은 아직 주석 처리 (나중에 smoothVisibility와 결합 필요)
                float3 toPixel = IN.positionWS - _ViewPosition.xyz;
                float distanceToView = length(toPixel);
                bool inCircleRange = distanceToView < _CircleRange;
                float3 viewDir = normalize(_ViewDirection.xyz);
                float3 pixelDir = normalize(toPixel);
                float angleBetween = acos(dot(viewDir, pixelDir)) * (180.0 / 3.14159265359);
                bool inViewCone = (distanceToView < _ViewRadius) && (angleBetween < _ViewAngle);
                if (inCircleRange || inViewCone)
                {
                    return half4(0,0,0,0);
                }
                else
                {
                    return _FogColor;
                }
                */
            }
            ENDHLSL
        }
    }
}
