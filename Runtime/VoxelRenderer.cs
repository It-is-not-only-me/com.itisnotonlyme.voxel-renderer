using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    [AddComponentMenu("Rendering/Voxel Renderer")]
    public class VoxelRenderer : MonoBehaviour
    {
        [SerializeField] private Material _material;

        private GenerarDatosBehaviour _generarDatos;
        

        private void Awake()
        {
            if (!TryGetComponent(out _generarDatos))
                Debug.LogError("No hay generador");
        }

        private void Update()
        {
            Render();
        }

        private void Render()
        {
            
        }
    }
}
