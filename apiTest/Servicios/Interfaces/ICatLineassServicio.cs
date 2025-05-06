using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatLineassServicio
    {
        Task<List<CatLineas>> Lista();
        Task<CatLineas> ObtenerPorLinea(int nLinea);
        Task<CatLineas> IME_CatLineas(CatLineas catLinea);
    }
}
