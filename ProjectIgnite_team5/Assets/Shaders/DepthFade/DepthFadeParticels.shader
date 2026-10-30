Shader "Custom/LighteningParticleWithFog"
{
    Properties
    {
        _BaseColor("Tint Color", Color) = (1, 1, 1, 1)
        _BaseMap("Particle Texture", 2D) = "white" {}
        _Intensity("Brightness Intensity", Float) = 1.0
        
        // Fog Properties
        _FogStart("Fog Start", Float) = 10.0
        _FogEnd("Fog End", Float) = 50.0
        _FogColor("Fog Tint Color", Color) = (0.5, 0.5, 0.5, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            // HARDWARE ADDITIVE LIGHTENING
            Blend One One
            ZWrite Off
            Cull Off
            ZTest LEqual

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float _Intensity;
                float _FogStart;
                float _FogEnd;
                float4 _FogColor;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.color = IN.color;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the particle texture
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                
                // Base particle RGB calculation
                half3 rgb = tex.rgb * IN.color.rgb * _BaseColor.rgb * _Intensity;
                rgb *= tex.a * IN.color.a;

                float2 screenUV = IN.positionHCS.xy / _ScaledScreenParams.xy;
                
                // Get scene depth behind the particle
                float rawDepth = SampleSceneDepth(screenUV);
                float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                float depthFactor = smoothstep(_FogStart, _FogEnd, eyeDepth);

                // Blend/tint the particle color into the fog color as it gets further away
                rgb = lerp(rgb, rgb * _FogColor.rgb, depthFactor);
                
                // if you want them to completely fade out in the fog, but does not seem te look nice
                // rgb *= (1.0 - depthFactor);

                return half4(rgb, 0.0);
            }

            ENDHLSL
        }
    }
}