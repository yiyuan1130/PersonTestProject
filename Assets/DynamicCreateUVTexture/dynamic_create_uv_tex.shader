Shader "EeamonnLi/dynamic_create_uv_tex" {
       
        SubShader {
                Tags { "RenderType"="Opaque" }        //设置替换渲染时的类型
                Cull Off        //关闭剪裁
               
                CGPROGRAM
                #pragma surface surf Lambert vertex:myVertex
                #pragma target 3.0

                //sampler2D _MainTex;

                struct Input {
                        float3 orginPosition;        //声明变量
                };
               
                //重写myvertex
                void myVertex(inout appdata_full v,out Input o)
                {
                        UNITY_INITIALIZE_OUTPUT(Input,o);
                        o.orginPosition=v.vertex.xyz;        //赋值
                }
               
                //重写surf方法
                void surf (Input IN, inout SurfaceOutput o) {
                        float4 niC=float4(0.763,0.657,0.614,0);                //定义变量
                        float4 zhuanC=float4(0.678,0.231,0.129,0);               
                        float4 colo=float4(0,0,1,0);
                       
                        //获得原始位置posself是4维的 用.xyz获得其中的3个
                        float3 currPos=IN.orginPosition;
                        float wd=asin(currPos.y);                        //当前片元的纬度
                        float jd=atan2(currPos.x,currPos.z);        //当前片元经度
                       
                        float yss=0.9999999;
                        float4 finalColor;
                       
                        int row=int(fmod((wd*100.0+90.0)/12.0,2.0));        //计算当前是位于奇数行还是偶数行
                        float ny=fmod(wd*100.0+90.0,12.0);        //判断当前片元是否为在此行的砖块垂直区间中的辅助变量
                       
                        float oeoffset=0.0;                //每行的砖块偏移值，奇数行偏移半个砖块
                        float nx;                //判断当前片元是否为在此列的砖块水平区间中的辅助变量
                        if(ny>10.0)
                        {
                                finalColor=niC;                //不在此行的砖块垂直区间中
                        }
                        else
                        {
                                if(row==1)
                                {
                                        oeoffset=11.0;                //若为奇数行则加上列偏移
                                }
                               
                                //计算当前是否在此列的砖块水平区间中的辅助变量
                                nx=fmod(jd*100+oeoffset,22.0);
                                if(abs(nx)>20.0)
                                {
                                        finalColor=niC;                //不在此列的砖块水平区间中
                                }
                                else
                                {
                                        finalColor=zhuanC;                        //在此列的砖块的水平区间中
                                }
                        }
                        o.Albedo=yss*finalColor;        //这里不能直接赋值，需要乘以一个接近1的小树，否则会报错的
                }
                ENDCG
        }
        FallBack "Diffuse"
}