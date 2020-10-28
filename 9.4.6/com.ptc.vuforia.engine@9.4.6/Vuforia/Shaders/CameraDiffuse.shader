//========================================================================
// Copyright (c) 2017 PTC Inc. All Rights Reserved.
//
// Vuforia is a trademark of PTC Inc., registered in the United States and other
// countries.
//=========================================================================

Shader "Custom/CameraDiffuse"
{
    Properties
    {
        _MaterialColor("Color", Color) = (1,1,1,1)
    }

        CGINCLUDE

        uniform float4 _MaterialColor;

    ENDCG

        SubShader
        {
            Pass
            {
            // indicate that our pass is the "base" pass in forward
            // rendering pipeline. It gets ambient and main directional
            // light data set up; light direction in _WorldSpaceLightPos0
            // and color in _LightColor0
            Tags {"LightMode" = "ForwardBase"}
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc" // for UnityObjectToWorldNormal
            #include "UnityLightingCommon.cginc" // for _LightColor0

            struct appdata
            {
                float4 position : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                fixed4 diff : COLOR0; // diffuse lighting color
                float4 position : POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v); //Insert
                UNITY_INITIALIZE_OUTPUT(v2f, o); //Insert
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //Insert

                o.position = UnityObjectToClipPos(v.position);


                // get vertex normal in world space
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);

                float3 worldPos = mul(unity_ObjectToWorld, v.position).xyz;
                // compute world space view direction
                float3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
                // dot product between normal and light direction for
                // standard diffuse (Lambert) lighting "(support double-sided material)" 
                half nl = abs(dot(worldNormal, worldViewDir));

                // factor in the material color
                o.diff = lerp(_MaterialColor, nl * _MaterialColor, 0.2);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return i.diff;
            }
            ENDCG
        }
    }
}