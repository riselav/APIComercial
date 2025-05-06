using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatTasaOCuotaServicio
    {
        Task<List<SatTasaOCuota>> Lista();
        Task<SatTasaOCuota> ObtenerPorTasaOCuota(string cValor);
    }
}
