// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

#include "UnityCG.cginc"
#include "UnityLightingCommon.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float3 normal : NORMAL;
	float4 tangent : TANGENT;
	float4 color : COLOR;
};

struct v2f
{
	float3 worldPos : TEXCOORD0;
	half3 tspace0 : TEXCOORD1;
	half3 tspace1 : TEXCOORD2;
	half3 tspace2 : TEXCOORD3;
	float2 uv : TEXCOORD4;	
	UNITY_FOG_COORDS(5)
	float4 vertex : SV_POSITION;
	fixed4 diff : COLOR0;
};

sampler2D _MainTex;
sampler2D _BumpMap;
float4 _Color;

float4 _MainTex_ST;
float _CurveStrength;

v2f vert(appdata v)
{
	v2f o;

	float _Horizon = 100.0f;
	float _FadeDist = 50.0f;

	o.vertex = UnityObjectToClipPos(v.vertex);

	float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

	o.vertex.y -= _CurveStrength * dist * dist * _ProjectionParams.x;

	o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	half3 wNormal = UnityObjectToWorldNormal(v.normal);
	half3 wTangent = UnityObjectToWorldDir(v.tangent.xyz);
	half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
	half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
	o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
	o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
	o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);

	half nl = max(0, dot(wNormal, _WorldSpaceLightPos0.xyz));
	o.diff = nl * _LightColor0;
	o.diff.rgb += ShadeSH9(half4(wNormal,1));
	o.diff.a = 1;

	o.uv = TRANSFORM_TEX(v.uv, _MainTex);

	UNITY_TRANSFER_FOG(o, o.vertex);

	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	//normal
	half3 tnormal = UnpackNormal(tex2D(_BumpMap, i.uv));
	half3 worldNormal;
	worldNormal.x = dot(i.tspace0, tnormal);
	worldNormal.y = dot(i.tspace1, tnormal);
	worldNormal.z = dot(i.tspace2, tnormal);
	half3 worldViewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
	half3 worldRefl = reflect(-worldViewDir, worldNormal);

	//difuse
	fixed4 col = tex2D(_MainTex, i.uv) * _Color;

	//ligth
	col *= i.diff;

	UNITY_APPLY_FOG(i.fogCoord, col);
	
	//return col;
	return col + (col * 0.5);
}