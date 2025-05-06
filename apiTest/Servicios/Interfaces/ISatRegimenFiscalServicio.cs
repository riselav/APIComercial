using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatRegimenFiscalServicio
    {
        Task<List<SatRegimenFiscal>> Lista();
        Task<SatRegimenFiscal> ObtenerPorId(long n_RegimenFiscal);
    }
}
