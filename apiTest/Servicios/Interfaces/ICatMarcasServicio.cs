using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatMarcasServicio
    {
        Task<List<CatMarcas>> Lista();
        Task<CatMarcas> ObtenerPorMarca(int nMarca);
        Task<CatMarcas> IME_CatMarcas(CatMarcas catMarca);
    }
}
