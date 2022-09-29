Shader "Unlit/StencilObjectUnlit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _ColorAware ("ColorAware", Color) = (1,1,1,1)
        _Step ("Step", float) = 5
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Fade" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 200

        Stencil {
            Ref 1
            Comp equal
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha:fade
            #pragma fragment frag alpha:fade
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
                fixed4 color: Color;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _ColorAware;
            float _Step;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.color = _Color;
                o.color = lerp(o.color, _ColorAware, step(_Step, v.vertex.z));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                return i.color;
            }
            ENDCG
        }
    }
}
