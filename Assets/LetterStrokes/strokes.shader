Shader "strokes"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RampTex ("RampTex", 2d) = "white" {}
		_RtTex("TRTex", 2D) = "white" {}

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		Blend SrcAlpha oneMinusSrcAlpha
		ZWrite Off
		LOD 100

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
				// float4 color : Color;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				// float4 color : TEXCOORD1;
				// UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _RampTex;
			sampler2D _RtTex;
			float4 _MainTex_ST;
			float4 _RampTex_ST;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				// o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 udf_col = tex2D(_MainTex, i.uv);
				float udf_v = udf_col.a;
					// 发光
				float2 ramp_uv = TRANSFORM_TEX(float2(udf_v, 0.5), _RampTex);
				fixed4 out_col = tex2D(_RampTex, ramp_uv);
				udf_col = out_col;
				float rtCol = tex2D(_RtTex, i.uv);
				if (udf_v > 0.6){
					udf_col.a = rtCol.r;
				}
				return udf_col;
			}
			ENDCG
		}
	}
}
