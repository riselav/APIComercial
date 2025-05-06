using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatSublineasServicio
    {
        Task<List<CatSublineas>> Lista();
        Task<CatSublineas> ObtenerPorSublinea(int nSublinea);
        Task<CatSublineas> IME_CatSublineas(CatSublineas catSublinea);
    }
}
