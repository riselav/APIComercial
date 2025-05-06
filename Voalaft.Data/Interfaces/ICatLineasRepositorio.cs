
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatLineasRepositorio
    {
        Task<List<CatLineas>> Lista();
        Task<CatLineas> ObtenerPorLinea(int nLinea);
        Task<CatLineas> IME_CatLineas(CatLineas catLinea);
    }
}
