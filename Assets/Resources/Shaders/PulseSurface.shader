Shader "Surface/PulseSurface" 
{
    Properties 
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _GlowColor ("Glow Color", Color ) = (1,1,1,1)
        _Frequency( "Glow Frequency", Float ) = 1.0
        _MinPulseVal( "Minimum Glow Multiplier", Range( 0, 1 ) ) = 0.5
        _MaxPulseVal( "Max Glow Multiplier", Range( 0, 1 ) ) = 1.0
        _EmissionMap("Emission", 2D) = "white" {}
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque"}
        LOD 200
    
        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows /*alpha:fade*/
        #pragma target 3.0

        sampler2D    _MainTex;
        sampler2D   _EmissionMap;

        fixed4      _GlowColor;
        half        _Glossiness;
        half        _Metallic;
        half        _Frequency;
        half        _MinPulseVal;
        half        _MaxPulseVal;

        struct Input 
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            half4 e = tex2D (_EmissionMap, IN.uv_MainTex);
            half posSin = 0.5 * sin( _Frequency * _Time.x ) + 0.5;
            half pulseMultiplier = posSin * ( _MaxPulseVal - _MinPulseVal ) + _MinPulseVal;

            o.Emission = e.rgb * _GlowColor * pulseMultiplier * _GlowColor.a;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            //o.Alpha = c.a * _GlowColor.a;
        }

        ENDCG
        
    }

    FallBack "Diffuse"
}