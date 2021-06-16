Shader "Unlit/HeatmapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridWidth("Grid width", Int) = 16
        _GridHeight("Grid height", Int) = 8
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
            int _GridWidth;
            int _GridHeight;
            Buffer<float> _GridValuesBuffer;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture based on buffer value
                int x = floor(i.uv[0] * _GridWidth);
                int y = floor(i.uv[1] * _GridHeight);
                int valueIndex = x + y * _GridWidth;
                float value = _GridValuesBuffer[valueIndex] * 0.01;
                fixed4 col = tex2D(_MainTex, float2(value, 0));
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
