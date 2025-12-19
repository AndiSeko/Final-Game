Shader "Custom/ASULOutline"
{
    Properties
    {
        _MainColor ("Outline Color", Color) = (0,0,0,1)
        _Thickness ("Base Thickness (near camera)", Range(0.001, 0.3)) = 0.05  
        _ScaleDistance ("Distance Scale Start", Range(1, 200)) = 30           
        _ScalePower ("Thickness Reduction Power", Range(0.1, 5)) = 2      
        _FadeDistance ("Transparency Fade Start", Range(1, 300)) = 80   
        _FadePower ("Transparency Fade Power", Range(0.5, 5)) = 1.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float depth : TEXCOORD0;
            };

            fixed4 _MainColor;
            float _Thickness;
            float _ScaleDistance;
            float _ScalePower;
            float _FadeDistance;
            float _FadePower;

            v2f vert (appdata v)
            {
                v2f o;

                float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));

                float depth = -viewPos.z;

                float thicknessScale = saturate(1.0 - pow(max(0, depth - _ScaleDistance) / _ScaleDistance, _ScalePower));
                thicknessScale = lerp(0.1, 1.0, thicknessScale); 

                float offset = _Thickness * thicknessScale;

                viewPos.xyz += viewNormal * offset;

                o.pos = mul(UNITY_MATRIX_P, viewPos);
                o.depth = depth;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fade = saturate(1.0 - pow(i.depth / _FadeDistance, _FadePower));

                fixed4 col = _MainColor;
                col.a *= fade;
                return col;
            }
            ENDCG
        }
    }
}