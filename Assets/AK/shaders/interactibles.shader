Shader "Unlit/interactibles"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeedX ("Scroll Speed X", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
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
            float _ScrollSpeedX;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Scroll the texture in the X-axis
                float offset = _Time.y * _ScrollSpeedX;
                o.uv = v.uv + float2(offset, 0);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture with the modified UV coordinates
                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}