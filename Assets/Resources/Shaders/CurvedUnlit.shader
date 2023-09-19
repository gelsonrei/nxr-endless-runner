Shader "Unlit/CurvedUnlit"
{ 
	Properties
	{
		_Color("Color", Color) = (.1, .1, .1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off
		Lighting On

		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_shadowreceiver
			#pragma multi_compile_fog
				
			#include "Unlit/CurvedCode.cginc"

			ENDCG
		}
	}

	FallBack "Diffuse"
}
