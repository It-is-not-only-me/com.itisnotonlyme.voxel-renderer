using UnityEngine;

namespace ItIsNotOnlyMe.VoxelRenderer
{
    public class GenerarDatosPerlinNoise : GenerarDatos
    {
        [SerializeField] private Vector3 _tamanio;
        [SerializeField] private Vector3Int _cantidadDePuntos;

        private Dato[,,] _datos;
        private Vector3Int _datosPorEje;

        public override Bounds Limites { get => new Bounds(transform.position, _tamanio); }

        public override Vector3Int DatosPorEje => _datosPorEje;

        private void Awake()
        {
            _datosPorEje = _cantidadDePuntos;
            _datos = new Dato[_datosPorEje.x, _datosPorEje.y, _datosPorEje.z];

            for (int i = 0; i < _datosPorEje.x; i++)
                for (int j = 0; j < _datosPorEje.y; j++)
                    for (int k = 0; k < _datosPorEje.z; k++)
                    {
                        _datos[i, j, k].Color = new Vector3(0, 0, 1);
                        _datos[i, j, k].Transparente = (j < _datosPorEje.y / 2) ? 0 : 1;
                    }
        }

        public override Dato[,,] Datos() => _datos;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Limites.center, Limites.size);
        }
    }
}
