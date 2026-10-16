
Shader "Custom/DepthFade"
{
	Properties
	{
		_BaseColor("Near Color", Color) = (1, 0, 1, 1)
		_FarColor("Far Color", Color) = (0, 0, 0, 1)
		_FogStart("Fade Start", Float) = 10.0
		_FogEnd("Fade End", Float) = 50.0
	}

	SubShader
	{
		Tags
		{
			"RenderPipeline" = "UniversalPipeline"
			"RenderType" = "Opaque"
		}

		Pass
		{
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

			CBUFFER_START(UnityPerMaterial)
				float4 _BaseColor;
				float4 _FarColor;
				float _FogStart;
				float _FogEnd;
			CBUFFER_END

			struct Attributes
			{
				float4 positionOS : POSITION;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
			};

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				return OUT;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				float2 screenUV = IN.positionHCS.xy / _ScaledScreenParams.xy;

				// get depth value
				float rawDepth = SampleSceneDepth(screenUV);

				// Convert the logrithmic depth range to a linear range to be used for ramping.
				float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

				//here we do the ramping.
				float depthFactor = smoothstep(_FogStart, _FogEnd, eyeDepth);

				// lerp color based on depthFactor
				return lerp(_BaseColor, _FarColor, depthFactor);
			}

			ENDHLSL
		}
	}
}
