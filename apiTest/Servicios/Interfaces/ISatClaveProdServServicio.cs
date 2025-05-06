using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatClaveProdServServicio
    {
        Task<List<SatClaveProdServ>> Lista();
        Task<SatClaveProdServ> ObtenerPorClaveProdServ(string c_ClaveProdServ);
    }
}
