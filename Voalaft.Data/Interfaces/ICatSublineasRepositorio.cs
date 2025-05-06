
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatSublineasRepositorio
    {
        Task<List<CatSublineas>> Lista();
        Task<CatSublineas> ObtenerPorSublinea(int nSublinea);
        Task<CatSublineas> IME_CatSublineas(CatSublineas catSublinea);
    }
}
