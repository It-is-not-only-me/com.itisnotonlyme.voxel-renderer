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
        }

        private Mesh GenerarMesh()
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[8];
            List<int> indices = new List<int>();
            Vector2[] uvs = new Vector2[8];

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
                        uvs[contador] = (new Vector2(i, j) + Vector2.one) / 2;
                    }

            AgregarIndices(4, 5, 7, 6, indices);
            AgregarIndices(5, 1, 3, 7, indices);
            AgregarIndices(1, 0, 2, 3, indices);
            AgregarIndices(0, 4, 6, 2, indices);
            AgregarIndices(7, 3, 2, 6, indices);
            AgregarIndices(4, 0, 1, 5, indices);
            
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Triangles, 0);
            mesh.SetUVs(0, uvs);

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
