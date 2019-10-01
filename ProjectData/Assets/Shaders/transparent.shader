﻿Shader "Custom/transparent"
{
	Properties 
    {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader 
    {
		Tags {"RenderType"="Transparent" "Queue"="Transparent"}
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha
        #pragma target 3.0

		sampler2D _MainTex;
        fixed4 _Color;

		struct Input 
        {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) 
        {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}