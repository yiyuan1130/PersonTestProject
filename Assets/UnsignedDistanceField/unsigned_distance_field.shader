Shader "iHuman/unsigned_distance_field"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_RampTex ("RampTex", 2d) = "white" {}
		_WidthIn ("WidthIn", range(0, 1)) = 0
		_WidthOut ("WidthOut", range(0, 1)) = 0

		_PureColor ("PureColor", color) = (1, 0, 0, 1)

		// 样式对应规则
		// 0 发光
		// 1 纯色
		[Enum(Rim,0,Pure,1)]_Style("Style", int) = 0

		// 通道对应：0:a 1:r 2:g 3:b
		// a通道：字母
		// r通道：第一笔
		// g通道：第二笔
		// b通道：第三笔
		[Enum(First,1,Second,2,Third,3,All,4)]_Step("Step", int) = 0
		
		_PureWidth ("PureWidth", range(0, 1)) = 0.5

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
				// UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _RampTex;
			float4 _MainTex_ST;
			float4 _RampTex_ST;
			int _Step;
			int _Style;
			fixed4 _PureColor;
			float _WidthIn;
			float _WidthOut;
			float _PureWidth;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 udf_col = tex2D(_MainTex, i.uv);
				float udf_v = 0;
				// 选择笔画
				switch (_Step){
					case 1:
						udf_v = udf_col.r;
						break;
					case 2:
						udf_v = udf_col.g;
						break;
					case 3:
						udf_v = udf_col.b;
						break;
					case 4:
						udf_v = udf_col.a;
						break;
					default:
						udf_v = 0;
						break;
				}

				// 选择样式
				if (_Style == 0){
					// 发光
					float2 ramp_uv = TRANSFORM_TEX(float2(udf_v - _WidthIn, 0.5), _RampTex);
					fixed4 out_col = tex2D(_RampTex, ramp_uv);
					// fixed4 col1 = tex2D(_RampTex, ramp_uv + float2(_WidthOut, _WidthOut));
					// fixed4 col2 = tex2D(_RampTex, ramp_uv + float2(-_WidthOut, _WidthOut));
					// fixed4 col3 = tex2D(_RampTex, ramp_uv + float2(_WidthOut, -_WidthOut));
					// fixed4 col4 = tex2D(_RampTex, ramp_uv + float2(-_WidthOut, -_WidthOut));
					// out_col = (col1 + col2 + col3 + col4 + out_col) / 5.0;
					udf_col.rgb = out_col.rgb;
				}
				else if (_Style == 1){
					// 纯色
					_PureWidth = 1 - _PureWidth;
					clip(udf_v - _PureWidth);
					udf_col.a = 1;
					udf_col.rgb = _PureColor.rgb;
				}
				if (udf_v == 0){
					udf_col = fixed4(0, 0, 0, 0);
				}
				return udf_col;
			}
			ENDCG
		}
	}
}
