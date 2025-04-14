Shader "Custom/PerlinNoiseScroll"
{
    Properties
    {
        _ScrollSpeedX ("Scroll Speed X", Float) = 3.0
        Scale ("Noise Scale", Float) = 1.00
        _Opacity  ("Opacity", Float) = 1.00
        _color ("Color", Color) = (1,1,1,1)
        _Texture ("Texture2D display name", 2D) = "red" {}
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_Position;
            };

            float _ScrollSpeedX;
            float4 _color;
            float Scale;
            float _Opacity;
            Texture2D  _Texture;
            // float4 _Time;
            
            // Linear interpolation
            float lerp(float a, float b, float t)
            {
                return a + (b - a) * t;
            }


            float2 unity_gradientNoise_dir(float2 p)
            {
                p = p % 289;
                float x = (34 * p.x + 1) * p.x % 289 + p.y;
                x = (34 * x + 1) * x % 289;
                x = frac(x / 41) * 2 - 1;
                return normalize(float2(x - floor(x + 0.5), abs(x) - 0.5));
            }

            float unity_gradientNoise(float2 p)
            {
                float2 ip = floor(p);
                float2 fp = frac(p);
                float d00 = dot(unity_gradientNoise_dir(ip), fp);
                float d01 = dot(unity_gradientNoise_dir(ip + float2(0, 1)), fp - float2(0, 1));
                float d10 = dot(unity_gradientNoise_dir(ip + float2(1, 0)), fp - float2(1, 0));
                float d11 = dot(unity_gradientNoise_dir(ip + float2(1, 1)), fp - float2(1, 1));
                fp = fp * fp * fp * (fp * (fp * 6 - 15) + 10);
                return lerp(lerp(d00, d01, fp.y), lerp(d10, d11, fp.y), fp.x);
            }

            float  Unity_GradientNoise_float(float2 UV)
            {
                return  ( unity_gradientNoise(UV * Scale) + 0.5);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Scroll the UV in the X-axis
                float offset = _Time.y * _ScrollSpeedX;
                o.uv = v.uv + float2(offset, 0);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Generate gradiant noise at the UV coordinate
                float n = Unity_GradientNoise_float(i.uv );  // Scale the UV for finer noise patterns
                // Return a purple color with the noise value
                float4 temp =  float4 (n * _color.x, n* _color.y, n*_color.z, n);
                return temp;
                return float4 (n * _color.x, n* _color.y, n*_color.z, n); 
                return fixed4(n * _color.x, n* _color.y, n*_color.z, n);

            }
            ENDCG   
        }
    }
}
