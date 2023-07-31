Shader "Custom/InvertWaveshapeButtonColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        // This is the button state, 0 is off, 1 is on
        IsOn ("IsOn", int) = 0
        Threshold ("Threshold", Range(0, 1)) = 0.5
        
        /** Stencil buffer settings  */
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            int IsOn;
            float Threshold;
            float grayScale(fixed4 col)
            {
               return (0.299f * col.r) + (0.587f * col.g) + (0.114f * col.b);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
           
                if (IsOn == 1 && Threshold > grayScale(col))
                {
                    // just invert the colors
                    col.rgb = 1 - col.rgb;
                    return col; 
                }
                else
                {
                    return col;
                }
      
            }
            ENDCG
        }
    }
}
