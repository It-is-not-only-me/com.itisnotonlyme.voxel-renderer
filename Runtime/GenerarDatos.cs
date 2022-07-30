using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public abstract class GenerarDatos : MonoBehaviour, IGenerarDatos
    {
        public abstract Bounds Limites { get; }
    }
}
