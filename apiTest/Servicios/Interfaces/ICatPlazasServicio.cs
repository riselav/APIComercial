using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatPlazasServicio
    {
        Task<List<CatPlaza>> Lista();

        Task<CatPlaza> ObtenerPorPlaza(int nPlaza);
    }
}