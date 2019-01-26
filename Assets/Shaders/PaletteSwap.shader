Shader "Unlit/PalletteSwap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    
        _ColorR ("ColorR", Color) = (1,0,0,1)
        _ColorG ("ColorG", Color) = (0,1,0,1)
        _ColorB ("ColorB", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorR;
            float4 _ColorG;
            float4 _ColorB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 texCol = tex2D(_MainTex, i.uv);
                UNITY_APPLY_FOG(i.fogCoord, texCol);

                fixed4 rMask = texCol.r * _ColorR;
                fixed4 gMask = texCol.g * _ColorG;
                fixed4 bMask = texCol.b * _ColorB;
                fixed4 mask = rMask + gMask + bMask;
                texCol.r = mask.r;
                texCol.g = mask.g;
                texCol.b = mask.b;

                // apply fog
                return texCol;
            }
            ENDCG
        }
    }
}
