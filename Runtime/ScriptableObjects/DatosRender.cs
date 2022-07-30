using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    [CreateAssetMenu(fileName = "Datos para render", menuName = "VoxelRenderer/Datos para render")]
    public class DatosRender : ScriptableObject, IDatosRender
    {
        [SerializeField] private Shader _geometryShader;

        public Shader GeometryShader() => _geometryShader;
    }
}
