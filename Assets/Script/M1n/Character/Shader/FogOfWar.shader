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

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                half4 _BaseColor;
                half4 _FogColor;
                float4 _ViewPosition;
                float _CircleRange;
                float _ViewRadius;
                float _ViewAngle;
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
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
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
                half4 baseColor = baseMap * _BaseColor;

                float3 toPixel = IN.positionWS - _ViewPosition.xyz;
                float distanceToView = length(toPixel);

                // 1. 원형 시야 검사
                bool inCircleRange = distanceToView < _CircleRange;

                // 2. 전방 시야 검사
                float3 viewDir = normalize(_ViewDirection.xyz);
                float3 pixelDir = normalize(toPixel);
                float angleBetween = acos(dot(viewDir, pixelDir)) * (180.0 / 3.14159265359); // 라디안을 도로 변환

                bool inViewCone = (distanceToView < _ViewRadius) && (angleBetween < _ViewAngle);

                // 3. 최종 시야 결정
                if (inCircleRange || inViewCone)
                {
                    return baseColor;
                }
                return _FogColor;
            }
            ENDHLSL
        }
    }
}