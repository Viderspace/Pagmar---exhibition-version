Shader "Unlit/StructuredBufferNoCompute Unlit"
{
    Properties
    {
        _EmissionDistance("_Emission Distance", Float) = 1.0
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _Color ("Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float emissionPower : TEXCOORD1;
            };

            fixed4 _Color;
            float width = 0;
            float height = 0;
            float lineWidth = 1;
            float shrinkFactor = 1;
            float maxAmp = 1;
            float minAmp = -1;
            float ampRange = 1;
            float4 lineColor = float4(1, 1, 1, 1);

            #ifdef UNITY_COMPILER_HLSL
            struct myObjectStruct
            {
                float objPosition;
            };

            StructuredBuffer<myObjectStruct> audioData;
            #endif

            CBUFFER_START(MyRarelyUpdatedVariables)
            float _EmissionDistance;
            float4 _EmissionColor;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float4 wvertex = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1));
                o.emissionPower = 0;

                #ifdef UNITY_COMPILER_HLSL
                float dist, power;

                [unroll]
                for (int i = 0; i < 1; i++) //only 2 spheres in scene
                {
                    dist = abs(distance(audioData[i].objPosition, wvertex.xyz));
                    power = 1 - clamp(dist / _EmissionDistance, 0.0f, 1.0f);
                    o.emissionPower += power;
                }
                #endif

                return o;
            }

            // fixed4 frag (v2f i) : SV_Target
            // {
            //     fixed4 col = _Color;
            //     col += _EmissionColor*i.emissionPower;
            //     return col;
            // }
            fixed4 frag(v2f i) : SV_Target
            {
                float3 id = i.vertex.xyz;
                const float x1 = (float)(id.x - 1);
                const float x2 = (float)(id.x + 1);
                const float y1 = ((audioData[(int)x1].objPosition) * height * ampRange) + height / 2;
                const float y2 = (audioData[(int)x2].objPosition * height * ampRange) + height / 2;
                const float m = (y1 - y2) / (x1 - x2);
                const float n = y1 - m * x1;

                const float dist = abs(m * (float)id.x - (float)id.y + n) / sqrt(m * m + 1);
                const bool isCloseEnoughToLine = dist <= lineWidth && abs(m) < 200 && abs(m) > 0.00001;

                const float ceil = (1 + max(maxAmp, ampRange)) * height / 2;
                const float floor = (1 + min(minAmp, -ampRange)) * height / 2;
                const bool outsideBounds = id.x <= 0 || id.x >= width - 1 || (float)id.y < floor || (float)id.y > ceil;

                if (outsideBounds)
                {
                    // Result[id.xy] = float4(0, 0, 0, 0);
                    return float4(0, 0, 0, 0);
                }

                const bool audioIsSilent = maxAmp <= 0.001;

                if (audioIsSilent)
                {
                    if ((float)id.y - height / 2 < lineWidth && (float)id.y - height / 2 > -lineWidth)
                    {
                        // Result[id.xy] = lineColor; // White line
                        return lineColor;
                    }
                    else
                    {
                        // Result[id.xy] = float4(0, 0, 0, 0); // Black background
                        return float4(0, 0, 0, 0);
                    }
                }

                if (isCloseEnoughToLine)
                {
                    return lineColor; //  line color
                }
                else
                {
                    return float4(0, 0, 0, 0); // Black background
                }
            }
            ENDCG
        }
    }
}