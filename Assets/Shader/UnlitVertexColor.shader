Shader "Custom/UnlitVertexColor"
{
    // 외부에서 조작 가능한 속성 추가
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _Alpha ("Transparency", Range(0,1)) = 0.5
    }
    
    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        // 투명도 지원을 위한 블렌딩 설정
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Properties에서 선언한 변수들을 CGPROGRAM에서 사용
            float4 _Color;
            float _Alpha;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                
                // 버텍스 컬러와 전역 컬러를 곱함
                o.color = v.color * _Color;
                o.color.a *= _Alpha; // 알파 값에 투명도 적용
                
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
    FallBack "Transparent/Diffuse"
}
