Shader "Test/letter_rim"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Radio ("Radio", range(0,1)) = 0.5
		_Color ("Color", color) = (1, 1, 1, 1)
		_Value("Value", range(0, 1)) = 0
		_ShowTex("ShowTex", 2D) = "" {}
	}
	SubShader
	{
		Blend One OneMinusSrcAlpha
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			sampler2D _MainTex;
			sampler2D _ShowTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Value;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.color = v.color;
				return o;
			}

			float _Radio;
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				// fixed4 col = tex2D(_ShowTex, i.uv);
				fixed4 col = tex2D(_MainTex, i.uv);
				clip(col.a-_Value);

				return col;
			}
			ENDCG
		}
	}
}

