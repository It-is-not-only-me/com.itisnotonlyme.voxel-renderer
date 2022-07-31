using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public struct Dato
    {
        public Vector3 Color;
        public int Transparente;
    }

    public interface IGenerarDatos
    {
        public Bounds Limites { get; }

        public Vector3Int DatosPorEje { get; }

        public Dato[,,] Datos();
    }
}
