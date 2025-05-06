using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatCatalogosServicio
    {
        Task<List<CatCatalogos>> Lista();
        Task<List<CatCatalogos>> ObtenerPorNombre(string cNombre);
    }
}
