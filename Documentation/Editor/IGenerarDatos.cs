using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public interface IGenerarDatos
    {
        public Bounds Limites { get; }

        public Vector3Int DatosPorEje { get; }

        public Dato[,,] Datos();
    }
}
