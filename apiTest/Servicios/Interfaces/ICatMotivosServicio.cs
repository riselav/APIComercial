using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatMotivosServicio
    {
        Task<List<CatMotivos>> Lista();
        Task<List<CatMotivos>> ObtenerPorTipoMotivo(int nTipoMotivo);
    }
}
