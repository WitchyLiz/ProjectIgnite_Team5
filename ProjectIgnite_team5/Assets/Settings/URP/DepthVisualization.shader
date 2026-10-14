Shader "Custom/DepthVisualization"
{   
    SubShader
    {
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
		#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
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

            float4 Frag (Varyings input) : SV_Target
            {
				float rawDepth = SampleSceneDepth(input.texcoord);
				float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);
				float grayscale = saturate(eyeDepth / 50.0);

				return float4(grayscale, grayscale, grayscale, 1.0);
}
            
            ENDHLSL
        }
    }
}
