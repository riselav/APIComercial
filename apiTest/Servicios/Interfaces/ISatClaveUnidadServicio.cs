using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatClaveUnidadServicio
    {
        Task<List<SatClaveUnidad>> Lista();
        Task<SatClaveUnidad> ObtenerPorClaveUnidad(string c_ClaveUnidad);
    }
}
