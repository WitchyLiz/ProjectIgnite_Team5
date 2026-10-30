
Shader "Custom/DepthFade"
{
	Properties
	{
		_BaseColor("Near Color", Color) = (1, 0, 1, 1)
		_FarColor("Far Color", Color) = (0, 0, 0, 1)
		_FogStart("Fade Start", Float) = 10.0
		_FogEnd("Fade End", Float) = 50.0
		_BaseMap("Alpha Mask", 2D) = "white" {}
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

			TEXTURE2D(_BaseMap);
			SAMPLER(sampler_BaseMap);

			CBUFFER_START(UnityPerMaterial)
				float4 _BaseColor;
				float4 _FarColor;
				float _FogStart;
				float _FogEnd;
			CBUFFER_END

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionHCS : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Varyings vert(Attributes IN)
			{
				Varyings OUT;
				OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
				OUT.uv = IN.uv;
				return OUT;
			}

			half4 frag(Varyings IN) : SV_Target
			{
				half4 mask = SAMPLE_TEXTURE2D(
					_BaseMap,
					sampler_BaseMap,
					IN.uv
				);
				
				//if the masks alpha is 0 just discard the pixel.
				clip(mask.a - 0.0001);

				float2 screenUV = IN.positionHCS.xy / _ScaledScreenParams.xy;

				// get depth value
				float rawDepth = SampleSceneDepth(screenUV);

				// Convert the logrithmic depth range to a linear range to be used for ramping.
				float eyeDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

				//here we do the ramping.
				float depthFactor = smoothstep(_FogStart, _FogEnd, eyeDepth);

				half4 color = lerp(_BaseColor, _FarColor, depthFactor);

				color.a = mask.a;
				
				return color;
			}

			ENDHLSL
		}
	}
}
