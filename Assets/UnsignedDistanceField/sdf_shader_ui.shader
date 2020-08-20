// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "iHuman/SDF/ui"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [PerRendererData] _StencilComp ("Stencil Comparison", Float) = 8
        [PerRendererData] _Stencil ("Stencil ID", Float) = 0
        [PerRendererData] _StencilOp ("Stencil Operation", Float) = 0
        [PerRendererData] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [PerRendererData] _StencilReadMask ("Stencil Read Mask", Float) = 255

        [PerRendererData] _ColorMask ("Color Mask", Float) = 15

        [PerRendererData] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

		_RampTex ("RampTex", 2d) = "white" {}

		// 样式对应规则
		// 0 发光
		// 1 纯色
		[Enum(Ramp,0,Pure,1,Edge,2)]_Style("Style", int) = 1

		// 通道对应：1:r 2:g 3:b 4:a
		// r通道：第一笔
		// g通道：第二笔
		// b通道：第三笔
		// a通道：字母
		[Enum(First,1,Second,2,Third,3,All,4)]_Step("Step", int) = 4
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

			sampler2D _RampTex;
			float4 _RampTex_ST;
			int _Step;
			int _Style;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				fixed4 udf_col = (tex2D(_MainTex, i.texcoord) + _TextureSampleAdd) * i.color;
				fixed4 base_udf_col = tex2D(_MainTex, i.texcoord);
				#ifdef UNITY_UI_CLIP_RECT
                udf_col.a *= UnityGet2DClipping(i.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (udf_col.a - 0.001);
                #endif

				float udf_v = 0;
				// 选择笔画
				switch (_Step){
					case 1:
						udf_v = base_udf_col.r;
						break;
					case 2:
						udf_v = base_udf_col.g;
						break;
					case 3:
						udf_v = base_udf_col.b;
						break;
					case 4:
						udf_v = base_udf_col.a;
						break;
					default:
						udf_v = 0;
						break;
				}

				// 选择样式
				if (_Style == 0){
					// 发光
					float2 ramp_uv = TRANSFORM_TEX(float2(udf_v, 0.5), _RampTex);
					fixed4 out_col = tex2D(_RampTex, ramp_uv);
					udf_col = out_col;
				}
				else if (_Style == 1){
					// 纯色
					// udf_col = i.color * _Color;
                    udf_col = i.color;
					clip(udf_v - 0.5);
				}
				else if (_Style == 2){
					fixed4 c = i.color * _Color;
					// 边缘
					udf_col = abs(udf_v - 0.5) < 0.05 ? c : fixed4(0, 0, 0, 0);
				}
				return udf_col;
            }
        ENDCG
        }
    }
}