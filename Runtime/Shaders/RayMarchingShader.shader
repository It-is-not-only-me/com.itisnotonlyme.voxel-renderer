Shader "Unlit/RayMarchingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _maxPasos ("MaxPasos", int) = 20
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

            #include "UnityCG.cginc"
            //#define MAX_LADOS 6
            #define MAX_HIJOS 27

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 posicion_mundo : TEXCOORD0;
                float3 direccion_camara : TEXCOORD1;
            };

            struct Interseccion 
            {
                float3 color;
                float distancia;
                int cantidadPasos;
            };

            struct Dato 
            {
                float3 color;
                int transparente;

                int3 tamanioHijos; // cuantos hijos por eje, tiene que ser una grilla regular
                int posicionesHijos[MAX_HIJOS]; 

                /*int direcciones[MAX_LADOS];
                
                int posicionesHijos[MAX_HIJOS];
                int cantidadHijos;*/
            };

            StructuredBuffer<Dato> _datos;
            
            int _datosParaEjeX;
            int _datosParaEjeY;
            int _datosParaEjeZ;

            float3 _tamanio;
            float3 _posicion;

            int _maxPasos;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posicion_mundo = mul(unity_ObjectToWorld, v.vertex);
                o.direccion_camara = -WorldSpaceViewDir(v.vertex);
                return o;
            }

            Interseccion colorEnCamino(float3 puntoDeInicio, float3 Direccion) 
            {
                Interseccion interseccion;

                puntoDeInicio -= _posicion - _tamanio / 2;

                interseccion.color = _datos[0].color;
                interseccion.distancia = 1;
                interseccion.cantidadPasos = 3;

                return interseccion;
            }

            float InverseLerp(float desde, float hasta, float valor) {
                return (valor - desde) / (hasta - desde);
            }

            float ReMap(float valorInicio, float valorFinal, float destInicio, float destFinal, float valor) {
                float t = InverseLerp(valorInicio, valorFinal, valor);
                return lerp(destInicio, destFinal, t);
            }

            float4 frag(v2f i) : SV_Target
            {
                float3 direccion = normalize(i.direccion_camara);

                Interseccion interseccion = colorEnCamino(i.posicion_mundo.xyz, direccion);

                clip(interseccion.distancia);

                //float profundidad = ReMap(1, _maxPasos, 1, 0, interseccion.cantidadPasos);

                return float4(interseccion.color, 1);
            }
            ENDCG
        }
    }
}
