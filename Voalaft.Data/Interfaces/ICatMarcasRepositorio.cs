
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatMarcasRepositorio
    {
        Task<List<CatMarcas>> Lista();
        Task<CatMarcas> ObtenerPorMarca(int nMarca);
        Task<CatMarcas> IME_CatMarcas(CatMarcas catMarca);
    }
}
