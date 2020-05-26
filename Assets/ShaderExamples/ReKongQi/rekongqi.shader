Shader "Learning/rekongqi"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("NoiseTextrue", 2D) = "white" {}
		_Strength ("Strength", range(0, 1)) = 0.5
		_Speed ("Speed", range(0, 2)) = 0.5
	}
	SubShader
	{
		Tags { "Queue"="AlphaTest" "IgnoreProjector"="true" "RenderType"="TransparentCutout" }
		Blend SrcAlpha oneMinusSrcAlpha
		// Cull Off
		ZWrite Off
		// LOD 100
		GrabPass
		{
			"_GrabTex"
		}
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 grabPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			sampler2D _GrabTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				// o.uv = v.uv;
				o.uv = TRANSFORM_TEX(v.uv, _NoiseTex);
				o.grabPos = ComputeGrabScreenPos(o.vertex);
				return o;
			}
			
			float _Strength;
			float _Speed;
			fixed4 frag (v2f i) : SV_Target
			{
				float4 noise = tex2D(_NoiseTex, i.uv.xy - _Time.xy * _Speed * 0.1);
				i.grabPos.xy += (noise.xz * 2 - 1) * 0.1 * _Strength;

				half4 grabCol = tex2Dproj(_GrabTex, i.grabPos);
				return grabCol;
			}
			ENDCG
		}
	}
}
