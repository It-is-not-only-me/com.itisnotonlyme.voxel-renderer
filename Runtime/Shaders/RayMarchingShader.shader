Shader "Unlit/RayMarchingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 direccion_camara : TEXCOORD0;
            };

            struct Interseccion {
                float3 color;
                float distancia;
                int cantidadPasos;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.direccion_camara = -WorldSpaceViewDir(v.vertex);
                return o;
            }

            Interseccion colorEnCamino(float3 puntoDeInicio, float3 Direccion) 
            {
                Interseccion interseccion;

                interseccion.color = float3(1, 1, 1);
                interseccion.distancia = 1;
                interseccion.cantidadPasos = 0;

                return interseccion;
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 direccion = normalize(i.direccion_camara);

                Interseccion interseccion = colorEnCamino(i.vertex, direccion);

                clip(interseccion.distancia);

                return float4(direccion, 1);
                //return float4(interseccion.color, 1);
            }
            ENDCG
        }
    }
}
