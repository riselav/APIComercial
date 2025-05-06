
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatClaveUnidadRepositorio
    {
        Task<List<SatClaveUnidad>> Lista();
        Task<SatClaveUnidad> ObtenerPorClaveUnidad(string c_ClaveUnidad);
    }
}
