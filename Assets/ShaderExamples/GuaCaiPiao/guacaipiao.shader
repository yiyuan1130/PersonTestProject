Shader "Learning/guacaipiao"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BlitTex ("BlitTexture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{"Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout"}
        Cull Off
		// ZWrite Off
        Pass
        {
            Tags{"LightMode" = "ForwardBase"}

            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Lighting.cginc"
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
			sampler2D _BlitTex;

			float4x4 paintCameraVP;

            struct a2v
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float2 uv : TEXCOORD2;
				float4 paintPos : TEXCOORD3;
            };

            v2f vert(a2v v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);

				float4 paintPos = mul(paintCameraVP, mul(unity_ObjectToWorld, v.vertex));
				paintPos /= paintPos.w;

				o.paintPos.xy = paintPos.xy * 0.5 + 0.5;

                return o;

            }

            fixed4 frag(v2f i) : SV_TARGET0
            {
                fixed4 texcolor = tex2D(_MainTex,i.uv);

				float mask = tex2D(_BlitTex, i.paintPos).r;
                float a = mask;
				return fixed4(texcolor.rgb, 1- mask);
            }
            ENDCG
		}
	}
}
