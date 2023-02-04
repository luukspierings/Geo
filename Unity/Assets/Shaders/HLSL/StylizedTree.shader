
Shader "URP/StylizedTree"
{

    Properties
    {
        Height("Height", Float) = 1.0
        Base("Base", Float) = 1.0
        _Offset("Offset", Float) = 0.0

        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
        _AlphaCutoff("Alpha Cutoff", Float) = 0.5
        _LightColor("Light color", Color) = (1, 1, 1)
        _DarkColor("Dark color", Color) = (0, 0, 0)
        _AmountOfColors("Amount of colors", Range (1, 20)) = 2

        _ShadowColor("Shadow color", Color) = (0, 0, 0)
        _ShadowStep("Shadow step", Float) = 0.0

        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0
    }
    SubShader
    {
        Tags{ "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        
        LOD 300
        
        // Pass
        // {
        //     Tags {"LightMode" = "DepthOnly"}
            
        //     ZWrite On
        //     ColorMask 0
        //     Cull Off
            
        //     HLSLPROGRAM
        //     #pragma prefer_hlslcc gles
        //     #pragma exclude_renderers d3d11_9x
        //     #pragma target 2.0

        //     #pragma multi_compile _ WRITE_NORMAL_BUFFER
        //     #pragma multi_compile _ WRITE_MSAA_DEPTH
        //     #pragma shader_feature _ALPHATEST_ON
        //     #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
        //     #pragma multi_compile_instancing
            
        //     #pragma require geometry

        //     #pragma geometry LitPassGeom
        //     #pragma vertex LitPassVertex
        //     #pragma fragment DepthPassFragment
        //     //#pragma hull hull
        //     //#pragma domain domain
        //     #define SHADOW
            
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        //     #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        //     #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

        //     #include "TreePass.hlsl"
        //     //#include "Tesselator.hlsl"
          


        //     half4 DepthPassFragment(Varyings input) : SV_TARGET{
        //         //Alpha(SampleAlbedoAlpha(input.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap)).a, _BaseColor, _Cutoff);
        //         return 0;
        //     }

        //     ENDHLSL
        // }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            //ONLY FOR FADE: Blend SrcAlpha OneMinusSrcAlpha
            Blend[_SrcBlend][_DstBlend]
            ZWrite [_ZWrite]
            Cull Off

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 4.0

            #pragma shader_feature _SPECULARHIGHLIGHTS_OFF
            #pragma shader_feature _GLOSSYREFLECTIONS_OFF
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _RECEIVE_SHADOWS_ON
            //#pragma shader_feature _ _RENDERING_CUTOUT _RENDERING_FADE

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            #pragma multi_compile_fog
            #pragma require geometry

            #pragma geometry Geometry
            #pragma vertex Vertex
            #pragma fragment Fragment
            //#pragma hull hull
            //#pragma domain domain

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            #include "TreePass.hlsl"
            //#include "Tesselator.hlsl"
            ENDHLSL

        }

         // Shadow caster pass. This pass renders a shadow map.
        // We treat it almost the same, except strip out any color/lighting logic
        Pass {

            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 4.0

            #pragma multi_compile_shadowcaster
 
            //#pragma geometry Geometry
            #pragma vertex Vertex
            #pragma fragment ShadowFragment

            // Define a special keyword so our logic can change if inside the shadow caster pass
            #define SHADOW_CASTER_PASS

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            #include "TreePass.hlsl"

            half4 ShadowFragment(Varyings input) : SV_TARGET{
                return 0;
            }

            ENDHLSL
        }



       
    }

}
