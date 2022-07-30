using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public class GenerarDatosPerlinNoise : GenerarDatos
    {
        [SerializeField] private Bounds _limites;

        public override Bounds Limites => _limites;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(_limites.center, _limites.size);
        }
    }
}
