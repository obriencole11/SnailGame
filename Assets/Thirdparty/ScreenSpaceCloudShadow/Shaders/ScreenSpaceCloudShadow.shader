// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ScreenSpaceCloudShadow"
{
	Properties
	{
		_MainTex ("Cloud Texture (R)", 2D) = "black" {}
		_ShadowFactor ("XYZ:ColorMultiplier", Vector) = (1,1,1,1)
		_CloudFactor ("XY:WindSpeed ZW:CloudTiling", Vector) = (0.05,0.05,2,2)
		_WorldSpaceCameraRay ("", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"ForceNoShadowCasting"="True"
		}
		Pass
		{
			Fog { Mode Off }
			ZWrite Off ZTest Always
			Blend Zero OneMinusSrcColor

			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile SSCS_ORTHO_OFF SSCS_ORTHO
			#pragma multi_compile SSCS_TOPDOWN_OFF SSCS_TOPDOWN
			
			#include "UnityCG.cginc"

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv  : TEXCOORD0;
				#if SSCS_ORTHO
				float3 pos_nearplane : TEXCOORD1;
				#else
				float3 ray : TEXCOORD1;
				#endif
			};

			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;

			uniform float4 _ShadowFactor;
			uniform float4 _CloudFactor;
			uniform float4 _WorldSpaceCameraRay;

			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.uv = v.texcoord.xy;

				// v is vertex of farplane quad
				float3 pos_world = mul (unity_ObjectToWorld, v.vertex);
				#if SSCS_ORTHO
				// Calculate world space near plane vertex position
				o.pos_nearplane = pos_world - _WorldSpaceCameraRay.xyz;
				#else
				// Calculate world space ray of farplane quad from eye.
				o.ray = pos_world - _WorldSpaceCameraPos;
				#endif

				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				// Calculate world position by linear depth
				#if SSCS_ORTHO
				float  depth = SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, i.uv );
				float3 world = _WorldSpaceCameraRay.xyz * depth + i.pos_nearplane;
				#else
				float  depth = Linear01Depth( SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, i.uv ) );
				float3 world = i.ray * depth + _WorldSpaceCameraPos;
				#endif

				// Convert world position to cloud uv
				#if SSCS_TOPDOWN
				float2 world_uv = world.xz;
				#else
				float2 world_uv = world.xy;
				#endif
				float2 cloud_uv = world_uv * 0.005f; // 0.005 is some magic value.
				float2 cloud_wind = _CloudFactor.xy;
				float2 cloud_tiling = _CloudFactor.zw;
				cloud_uv = (cloud_uv + cloud_wind) * cloud_tiling;

				float  cloud = tex2D(_MainTex, cloud_uv).r;		// use R channel only
				float  cloud_faded = cloud * (1.0 - depth);		// fade by depth (don't render shadow to faraway skybox..)

				return float4(_ShadowFactor.xyz * cloud_faded, 1.0);
			}
			ENDCG
		}
	}
}
