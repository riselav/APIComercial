using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatCorreoContactoRFCServicio
    {
        Task<List<CatCorreoContactoRFC>> ObtenerPorId(long n_IDRFC);
    }
}