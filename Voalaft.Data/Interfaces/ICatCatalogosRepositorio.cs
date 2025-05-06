
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatCatalogosRepositorio
    {
        Task<List<CatCatalogos>> Lista();
        Task<List<CatCatalogos>> ObtenerPorNombre(string cNombre);
    }
}
