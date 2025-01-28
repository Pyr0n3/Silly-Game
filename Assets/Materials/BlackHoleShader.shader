Shader "Custom/BlackHoleLensing"
{
    Properties
    {
        _MainTex("Captured Scene", 2D) = "black" {}
        _BlackHolePos("Black Hole Position", Vector) = (0.5, 0.5, 0, 0)
        _EventHorizon("Event Horizon Radius", Range(0.05, 0.3)) = 0.1
        _DistortionStrength("Distortion Strength", Range(0, 1)) = 0.5
    }
        SubShader
        {
            Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
            Pass
            {
                Blend SrcAlpha OneMinusSrcAlpha  // Make the shader transparent
                ZTest Always Cull Off ZWrite Off

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float4 _BlackHolePos;
                float _EventHorizon;
                float _DistortionStrength;

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float2 DistortUV(float2 uv, float2 blackHoleCenter, float eventHorizon, float strength)
                {
                    float2 dir = uv - blackHoleCenter;
                    float dist = length(dir);
                    dir = normalize(dir);

                    if (dist < eventHorizon * 2.0)
                    {
                        float distortionAmount = strength * (1.0 / (dist + 0.1));
                        uv -= dir * distortionAmount * smoothstep(eventHorizon, eventHorizon * 2.0, dist);
                    }
                    return uv;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 blackHoleCenter = _BlackHolePos.xy;
                    float2 distortedUV = DistortUV(i.uv, blackHoleCenter, _EventHorizon, _DistortionStrength);

                    // Fix: Ensure the shader samples the texture properly
                    fixed4 sceneColor = tex2D(_MainTex, distortedUV);

                    // If the RenderTexture isn't assigned, fallback to original UVs
                    if (sceneColor.a == 0)
                    {
                        sceneColor = tex2D(_MainTex, i.uv);
                    }

                    return sceneColor;
                }
                ENDCG
            }
        }
}
