Shader "Learning/liuguang"
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
			
			sampler2D _MainTex;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float x = i.uv.x * 4 + i.uv.y;
				float light = smoothstep(0.98, 1.0, sin(x / 4 - _Time.w * 1.5));
				float a = light * 0.7;
				float3 v = lerp(col.rgb, float3(1, 1, 1), a);
				return fixed4(v, 1);
			}
			ENDCG
		}
	}
}

