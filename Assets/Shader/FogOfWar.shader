Shader "Custom/FogOfWar"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _FogTex ("Fog of War Texture", 2D) = "black" {}
        _ViewPosition ("View Position", Vector) = (0, 0, 0, 0)
        _ViewRange ("View Range", Float) = 10.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _FogTex;
            float4 _ViewPosition;
            float _ViewRange;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 worldPos : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float3 worldPos : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = v.worldPos;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // �þ� ���� ��� (�÷��̾���� �Ÿ�)
                float distanceToView = distance(i.worldPos, _ViewPosition.xyz);

                // �þ� ���� ���� ��� ���� �ؽ�ó ǥ��
                if (distanceToView < _ViewRange)
                {
                    return tex2D(_MainTex, i.worldPos.xy); // ���� �ؽ�ó
                }
                else
                {
                    // �þ� ���� ��� Fog �ؽ�ó�� �帮�� ó��
                    return tex2D(_FogTex, i.worldPos.xy); // Fog �ؽ�ó
                }
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
