Shader "Learning/clip"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Range(0, 1)) = 1
	}
	SubShader
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="true" "RenderType"="TransparentCutout"}
		ZWrite On
		Cull Off

		Blend SrcAlpha oneMinusSrcAlpha
		// Blend Off
		// BlendOp Max
		// BlendOp Min

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
				o.uv = v.uv + sin(_Time.y);
				return o;
			}
			
			sampler2D _MainTex;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				float3 line1 = float3(1.0 - 0.0, 0, 1.0 - 0.0);
				float3 p1 = float3(i.uv.x - 0.0, 0, i.uv.y - 0.0);
				float3 v1 = cross(p1, line1);
				float3 line2 = float3(0.0 - 1.0, 0, 1.0 - 0.0);
				float3 p2 = float3(i.uv.x - 1.0, 0, i.uv.y - 0.0);
				float3 v2 = cross(p2, line2);
				float r = min(length(v1), length(v2));
				clip(r - 0.04);
				return fixed4(r, 0, 0, r + 0.3);

				// float3 p1 = float3(i.uv.x - 0.0, 0, i.uv.y - 0.0);
				// float d1 = distance(p1, float3(0, 0, 0));
				// float d2 = distance(p1, float3(0, 0, 1));
				// float d3 = distance(p1, float3(1, 0, 0));
				// float d4 = distance(p1, float3(1, 0, 1));
				// clip(d1 - 0.3);
				// clip(d2 - 0.3);
				// clip(d3 - 0.3);
				// clip(d4 - 0.3);
				// return fixed4(i.uv.x,i.uv.y,0, 1);
			}
			ENDCG
		}
	}
}

