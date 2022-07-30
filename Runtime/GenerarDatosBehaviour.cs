using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public abstract class GenerarDatosBehaviour : MonoBehaviour, IGenerarDatos
    {
        public abstract Bounds Limites { get; }
    }
}
