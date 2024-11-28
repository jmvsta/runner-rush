Shader "Custom/AnimatedSkybox"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Columns ("Columns", Float) = 4
        _Rows ("Rows", Float) = 4
        _Speed ("Animation Speed", Float) = 1.5
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Background"
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Columns;
            float _Rows;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                uint totalFrames = _Columns * _Rows;
                
                float time = frac(_Time.y * _Speed) * totalFrames;
                uint frameIndex = (int)time;
                
                uint col = frameIndex - frameIndex / 4 * 4;
                uint row = frameIndex / 4;
                float2 frameUV = float2(
                    col * 0.25,
                    (3 - row) * 0.25
                );

                float2 uv = i.uv / float2(4, 4) + frameUV;

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}