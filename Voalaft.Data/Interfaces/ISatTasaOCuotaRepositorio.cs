
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatTasaOCuotaRepositorio
    {
        Task<List<SatTasaOCuota>> Lista();
        Task<SatTasaOCuota> ObtenerPorTasaOCuota(string cValor);
    }
}
