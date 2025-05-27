using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatDenominacionServicio
    {
        Task<List<CatDenominacion>> Lista();
        Task<CatDenominacion> ObtenerPorId(long n_Denominacion);
    }
}
