Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _Thickness ("Outline Thickness (pixels)", Range(1,10)) = 2
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _Thickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 duv = fwidth(i.uv) * _Thickness;

                float edge =
                    step(i.uv.x, duv.x) +
                    step(i.uv.y, duv.y) +
                    step(1.0 - i.uv.x, duv.x) +
                    step(1.0 - i.uv.y, duv.y);

                if (edge < 1.0)
                    discard;

                return _OutlineColor;
            }
            ENDCG
        }
    }

    FallBack Off
}