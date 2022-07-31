using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    [AddComponentMenu("Rendering/Voxel Renderer")]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class VoxelRenderer : MonoBehaviour
    {
        [SerializeField] private Material _material;
        [SerializeField] private DatosRender _datosRender;

        private GenerarDatos _generarDatos;
        private ComputeBuffer _datosBuffer;

        private int _datosStride = 3 * sizeof(float) + sizeof(int);

        private void Awake()
        {
            Material nuevoMaterial = new Material(_datosRender.GeometryShader());
            nuevoMaterial?.CopyPropertiesFromMaterial(_material);
            _material = nuevoMaterial;

            if (!TryGetComponent(out _generarDatos))
                Debug.LogError("No hay generador");

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = _material;

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = GenerarMesh();

            Actualizar();
        }

        private void Actualizar()
        {
            Vector3Int datosPorEje = _generarDatos.DatosPorEje;

            int cantidadDeDatos = datosPorEje.x * datosPorEje.y * datosPorEje.z;
            _datosBuffer = new ComputeBuffer(cantidadDeDatos, _datosStride);

            Dato[,,] datosDados = _generarDatos.Datos();
            Dato[] datosFinales = new Dato[cantidadDeDatos];

            for (int i = 0, contador = 0; i < datosPorEje.x; i++)
                for (int j = 0; j < datosPorEje.y; j++)
                    for (int k = 0; k < datosPorEje.z; k++, contador++)
                    {
                        datosFinales[contador] = datosDados[i, j, k];
                    }

            _datosBuffer.SetData(datosFinales);

            float[] tamanio = new float[3], posicion = new float[3];
            Bounds limites = _generarDatos.Limites;
            for (int i = 0; i < 3; i++)
            {
                tamanio[i] = limites.size[i] * 2;
                posicion[i] = limites.center[i];
            }

            _material.SetBuffer("_datos", _datosBuffer);
            _material.SetInt("_datosParaEjeX", datosPorEje.x);
            _material.SetInt("_datosParaEjeY", datosPorEje.y);
            _material.SetInt("_datosParaEjeZ", datosPorEje.z);
            _material.SetFloatArray("_tamanio", tamanio);
            _material.SetFloatArray("_posicion", posicion);
        }

        private void OnDestroy()
        {
            _datosBuffer.Dispose();
        }

        private Mesh GenerarMesh()
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[8];
            List<int> indices = new List<int>();

            Bounds limites = _generarDatos.Limites;

            for (int i = -1, contador = 0; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2, contador++)
                    {
                        Vector3 posicionRelativa = new Vector3
                        (
                            limites.size.x * i / 2,
                            limites.size.y * j / 2,
                            limites.size.z * k / 2
                        );

                        vertices[contador] = limites.center + posicionRelativa;
                    }

            AgregarIndices(4, 5, 7, 6, indices);
            AgregarIndices(5, 1, 3, 7, indices);
            AgregarIndices(1, 0, 2, 3, indices);
            AgregarIndices(0, 4, 6, 2, indices);
            AgregarIndices(7, 3, 2, 6, indices);
            AgregarIndices(4, 0, 1, 5, indices);
            
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);

            mesh.RecalculateNormals();
            mesh.bounds = limites;

            return mesh;
        }

        private void AgregarIndices(int indice0, int indice1, int indice2, int indice3, List<int> indices)
        {
            indices.Add(indice0);
            indices.Add(indice2);
            indices.Add(indice1);
            indices.Add(indice0);
            indices.Add(indice3);
            indices.Add(indice2);
        }
    }
}
