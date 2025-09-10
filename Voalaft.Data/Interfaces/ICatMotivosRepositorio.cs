
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatMotivosRepositorio
    {
        Task<List<CatMotivos>> Lista();
        Task<List<CatMotivos>> ObtenerPorTipoMotivo(int nTipoMotivo);
    }
}
