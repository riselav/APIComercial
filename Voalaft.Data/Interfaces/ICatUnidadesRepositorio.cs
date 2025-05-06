
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatUnidadesRepositorio
    {
        Task<List<CatUnidades>> Lista();
        Task<CatUnidades> ObtenerPorUnidad(int nUnidad);
        Task<CatUnidades> IME_CatUnidades(CatUnidades catUnidad);
    }
}
