using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatUnidadesServicio
    {
        Task<List<CatUnidades>> Lista();
        Task<CatUnidades> ObtenerPorUnidad(int nUnidad);
        Task<CatUnidades> IME_CatUnidades(CatUnidades catUnidad);
    }
}
