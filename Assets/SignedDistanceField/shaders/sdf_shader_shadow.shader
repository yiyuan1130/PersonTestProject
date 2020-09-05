Shader "iHuman/SDF/shadow"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", color) = (1, 1, 1, 1)
        _ShadowColor ("ShadowColor", color) = (0, 0, 0, 1)
        _ShadowOffset ("ShadowOffset", vector) = (0.02, 0.02, 0, 0)
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
            fixed4 _ShadowColor;
            float4 _ShadowOffset;

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
				fixed sdf_v = tex2D(_MainTex, i.uv).a;
                fixed4 udf_col = _Color;
                udf_col.a = smoothstep(0.5, 0.6, sdf_v);

                float shadow_v = tex2D(_MainTex, i.uv + _ShadowOffset.xy).a;
                fixed4 shadow_col = _ShadowColor;
                float delta = length(_ShadowOffset)*2;
                shadow_col.a = smoothstep(0.5 - delta, 0.6 + delta, shadow_v);

                return lerp(shadow_col, udf_col, udf_col.a);

			}
			ENDCG
		}
	}
}
