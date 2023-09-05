Shader "Unlit/CurvedUnlitAlpha"
{
	Properties
	{
		_Color("Color", Color) = (.1, .1, .1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_BumpMap("Normal Map", 2D) = "bump" {}
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Lighting On

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB

		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

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
