Shader "UI/Test"
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
                float2 aspectRatio : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Radius;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // UI のアスペクト比を取得
                float2 scale = float2(length(UNITY_MATRIX_IT_MV[0].xyz), length(UNITY_MATRIX_IT_MV[1].xyz));
                float aspect = scale.y / scale.x;

                // マスク用のアスペクト比補正を適用
                o.aspectRatio = float2(aspect, 1);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float radius = _Radius;

                // 画像のUVはそのまま使用（引き伸ばさない）
                fixed4 col = tex2D(_MainTex, uv);

                // 角丸のマスク処理（座標補正のみ）
                float2 maskUV = (uv - 0.5) * i.aspectRatio + 0.5;
                float2 dist = abs(maskUV - 0.5) - 0.5 + radius;
                
                if (dist.x > 0 && dist.y > 0 && dist.x * dist.x + dist.y * dist.y > radius * radius)
                {
                    col.a = 0; // 透明にする
                }

                return col;
            }
            ENDCG
        }
    }
}
