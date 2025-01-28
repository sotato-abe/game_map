Shader "UI/RoundedCorners"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Corner Radius", Float) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Radius;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // UV座標を計算
                float2 uv = i.uv;

                // カードの角丸の処理
                float radius = _Radius;
                float2 dist = abs(uv - 0.5) - 0.5 + radius;

                // 透明部分を作成
                if (dist.x > 0 && dist.y > 0 && dist.x * dist.x + dist.y * dist.y > radius * radius)
                {
                    return float4(0, 0, 0, 0); // 完全に透明にする
                }

                return tex2D(_MainTex, uv);
            }
            ENDCG
        }
    }
}
