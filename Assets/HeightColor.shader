Shader "Custom/HeightColor"
{
    Properties
    {
        _Color0("Color 1", Color) = (0, 0, 1, 1) 
        _Color1("Color 2", Color) = (0.9, 0.8, 0.4, 1) 
        _Color2("Color 3", Color) = (0.2, 0.6, 0.2, 1) 
        _Color3("Color 4", Color) = (0.5, 0.5, 0.5, 1) 
        _Color4("Color 5", Color) = (1, 1, 1, 1) 
        _Layer1("Layer 1", Float) = 0.01
        _Layer2("Layer 2", Float) = 0.05 
        _Layer3("Layer 3", Float) = 0.5 
        _Layer4("Layer 4", Float) = 0.9 
        _HeightTerrein("Height of terrein", Float) = 600.0 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            // Properties
            float4 _Color0;
            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float _Layer1;
            float _Layer2;
            float _Layer3;
            float _Layer4;
            float _HeightTerrein;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float height : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.height = v.vertex.y;  //  lokalna wysokosc
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float height = i.height;

                float Uni_Height = height / _HeightTerrein; 

                half4 new_color;

                if (Uni_Height < _Layer1)
                {
                    new_color = _Color0; 
                }
                else if (Uni_Height < _Layer2)
                {
                    new_color = _Color1; 
                }
                else if (Uni_Height < _Layer3)
                {
                    new_color = _Color2;
                }
                else if (Uni_Height < _Layer4)
                {
                    new_color = _Color3; 
                }
                else
                {
                    new_color = _Color4; 
                }

                return new_color;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
