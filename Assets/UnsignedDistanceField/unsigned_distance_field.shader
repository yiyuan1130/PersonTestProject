Shader "iHuman/unsigned_distance_field"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RampTex ("RampTex", 2d) = "white" {}
		_WidthIn ("WidthIn", range(0, 1)) = 0
		_WidthOut ("WidthOut", range(0, 1)) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		Blend SrcAlpha oneMinusSrcAlpha
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _RampTex;
			float4 _MainTex_ST;
			float4 _RampTex_ST;
			float _WidthIn;
			float _WidthOut;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 usdf_col = tex2D(_MainTex, i.uv);
				float2 ramp_uv = TRANSFORM_TEX(float2(usdf_col.a - _WidthIn, 0.5), _RampTex);
				fixed4 out_col = tex2D(_RampTex, ramp_uv);
				fixed4 col1 = tex2D(_RampTex, ramp_uv + float2(_WidthOut, _WidthOut));
				fixed4 col2 = tex2D(_RampTex, ramp_uv + float2(-_WidthOut, _WidthOut));
				fixed4 col3 = tex2D(_RampTex, ramp_uv + float2(_WidthOut, -_WidthOut));
				fixed4 col4 = tex2D(_RampTex, ramp_uv + float2(-_WidthOut, -_WidthOut));
				out_col = (col1 + col2 + col3 + col4 + out_col) / 5.0;
				usdf_col.rgb = out_col.rgb;
				return usdf_col;
			}
			ENDCG
		}
	}
}
