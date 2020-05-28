Shader "Learning/blit"
{
	Properties
	{
		_BrushWidth ("BrushWidth", int(1, 3)) = 2
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Blend One One

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _CurrentRT;
			sampler2D _PrevirousRT;
			int _BrushWidth;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 cur = tex2D(_CurrentRT, i.uv);
				fixed4 pre = tex2D(_PrevirousRT, i.uv);
				float r = cur.r - pre.r;
				return fixed4(r / _BrushWidth, 0, 0, 1);
			}
			ENDCG
		}
	}
}
