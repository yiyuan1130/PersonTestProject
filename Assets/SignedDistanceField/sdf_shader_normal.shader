Shader "iHuman/SDF/normal"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
		_RampTex ("RampTex", 2d) = "white" {}

		// 样式对应规则
		// 0 发光
		// 1 纯色
		[Enum(Ramp,0,Pure,1,Edge,2,Shadow,3)]_Style("Style", int) = 1

		// 通道对应：1:r 2:g 3:b 4:a
		// r通道：第一笔
		// g通道：第二笔
		// b通道：第三笔
		// a通道：字母
		[Enum(First,1,Second,2,Third,3,All,4)]_Step("Step", int) = 4

		_DistanceMark("DistanceMark", range(0,1)) = 1
		_SmoothDelta("SmoothDelta", range(0,0.1)) = 0

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
			float4 _MainTex_ST;
			float4 _RampTex_ST;
			int _Step;
			int _Style;
			fixed4 _Color;
			float _DistanceMark;
			float _SmoothDelta;

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
				float udf_v = 0;
				// float arr[4] = {udf_col.r, udf_col.g, udf_col.b, udf_col.a};
				// udf_v = arr[_Step - 1];
				if (_Step == 1){
					udf_v = udf_col.r;
				}
				else if (_Step == 2) {
					udf_v = udf_col.g;
				}
				else if (_Step == 3) {
					udf_v = udf_col.b;
				}
				else if (_Step == 4) {
					udf_v = udf_col.a;
				}
				// udf_v = smoothstep(_DistanceMark - _SmoothDelta, _DistanceMark + _SmoothDelta, udf_v);


				// 选择笔画
				// switch (_Step){
				// 	case 1:
				// 		udf_v = udf_col.r;
				// 		break;
				// 	case 2:
				// 		udf_v = udf_col.g;
				// 		break;
				// 	case 3:
				// 		udf_v = udf_col.b;
				// 		break;
				// 	case 4:
				// 		udf_v = udf_col.a;
				// 		break;
				// 	default:
				// 		udf_v = 0;
				// 		break;
				// }

				// 选择样式
				if (_Style == 0){
					// 发光
					float2 ramp_uv = TRANSFORM_TEX(float2(udf_v, 0.5), _RampTex);
					fixed4 out_col = tex2D(_RampTex, ramp_uv);
					udf_col = out_col;
				}
				else if (_Style == 1){
					// 纯色
					udf_col = _Color;
					clip(udf_v - 0.5);
				}
				else if (_Style == 2){
					fixed4 c = _Color;
					// 边缘
					udf_col.a = abs(udf_v - 0.5) < 0.05 ? c : fixed4(0, 0, 0, 0);
				}
				else if (_Style == 3){
					float2 _Offset = float2(0, 0.05);
					float board = 0.5;
					fixed4 udf_col_2 = tex2D(_MainTex, i.uv + _Offset);
					udf_col_2 = udf_col_2.a > board ? fixed4(1, 1, 1, 1) : fixed4(0, 0, 0, 0);
					udf_col = udf_col.a > board ? fixed4(1, 1, 1, 1) : fixed4(0, 0, 0, 0);
					udf_col.rgb = lerp(udf_col.rgb, udf_col_2.rgb, 0.3);
				}
				udf_col.a = smoothstep(_DistanceMark - _SmoothDelta, _DistanceMark + _SmoothDelta, udf_v);
				return udf_col;
			}
			ENDCG
		}
	}
}
