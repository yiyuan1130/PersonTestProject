Shader "Unlit/liuguang"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed ("Speed", range(-2, 2)) = 1.04
		_Width ("Width", range(1, 10)) = 5.83
		_Angle ("Angle", range(-1, 1)) = 0.33
		_Light ("Light", range(0, 1)) = 0.51
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float _Speed;
			float _Width;
			float _Angle;
			float _Light;
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float x = i.uv.x + i.uv.y * _Angle;
				float v = sin(x - _Time.w * _Speed);
				v = smoothstep(1 - _Width / 1000, 1.0, v);
				float3 target = float3(v, v, v) + col.rgb;
				col.rgb = lerp(col.rgb, target, _Light);
				return fixed4(target, 1);
			}
			ENDCG
		}
	}
}
