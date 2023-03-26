Shader "Custom/Chrome Shader"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Culling", Int) = 0
        [ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
        [ToggleOff] _GlossyReflections("Glossy Reflections", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200
        ZWrite On
        Cull[_Cull]

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        #pragma shader_feature _ _SPECULARHIGHLIGHTS_OFF
        #pragma shader_feature _ _GLOSSYREFLECTIONS_OFF

        sampler2D _NormalMap;

        struct Input
        {
            float2 uv_NormalMap;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            o.Albedo = _Color.rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Emission = _Color.rgb / 4;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
