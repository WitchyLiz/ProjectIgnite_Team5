Shader "Custom/DepthVisualization"
{   
	Properties
	{
        _MonochromeStrength ("Monochrome Strength", Range(0, 1)) = 1.0
        _FogColor ("Fog Color", Color) = (0.7, 0.7, 0.7, 1)
        _FogStart ("Fog Start", Float) = 10.0
        _FogEnd ("Fog End", Float) = 50.0
        _DepthColor ("Depth Color", Color) = (1, 1, 1, 1)
	}
	
    SubShader
    {
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

        CBUFFER_START(UnityPerMaterial)
            float _MonochromeStrength;
            float4 _FogColor;
            float _FogStart;
            float _FogEnd;
            float4 _DepthColor;
        CBUFFER_END

        ENDHLSL

        Tags { "RenderType"="Opaque" }
        LOD 100
        ZWrite Off Cull Off
        Pass
        {
            Name "DepthVisualization"

            HLSLPROGRAM
            
            #pragma vertex Vert
            #pragma fragment Frag

            float4 Frag(Varyings input) : SV_Target
            {
                // Original color
                float4 sceneColor = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    input.texcoord
                );

                // get z buffer value
                float rawDepth = SampleSceneDepth(input.texcoord);

                // Skybox is alyways rendered with the farthest depth value
                // Leave the skybox unchanged
                //if (rawDepth <= 0.0000001)
                //    return sceneColor;
               

                // Convert the logrithmic depth range to a linear range to be used for ramping.
                float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
 
                //here we do the ramping and flip the values
                float depthFactor = smoothstep(_FogStart, _FogEnd, eyeDepth);


                //We do lerp the pixel value between the color tint we want and the depth value
                //closer is black, farther is our tint
                // could make closer an other color than black
                float3 color = lerp(
                    float3(0, 0, 0),
                    _FogColor.rgb,
                    depthFactor
                );

                // yay we win!!
                return float4(color, 1.0);
            }
            
            ENDHLSL
        }
    }
}