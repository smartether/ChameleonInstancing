Shader "Unlit/ChameleonInstancing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MeshID("MeshID", Float) = 0
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
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #pragma target 3.0
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv8 : TEXCOORD7;
                DEFAULT_UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            UNITY_INSTANCING_BUFFER_START(_Chameleon)
            UNITY_DEFINE_INSTANCED_PROP(float, _MeshID)
            UNITY_INSTANCING_BUFFER_END(_Chameleon)

            v2f vert (appdata v)
            {
                UNITY_SETUP_INSTANCE_ID(v);
                v2f o;
                float meshID = UNITY_ACCESS_INSTANCED_PROP(_Chameleon, _MeshID);
                float fclip = saturate(1000 * abs(v.uv8.x - meshID));
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.y += fclip * (1.00 + abs(o.vertex.y)) * o.vertex.w;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color.r = fclip;// = v.uv8.rg;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
