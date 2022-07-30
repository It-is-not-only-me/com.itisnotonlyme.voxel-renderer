using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public class GenerarDatosPerlinNoise : GenerarDatos
    {
        [SerializeField] private Vector3 _tamanio;

        public override Bounds Limites { get => new Bounds(transform.position, _tamanio); }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Limites.center, Limites.size);
        }
    }
}
