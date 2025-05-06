
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatClaveProdServRepositorio
    {
        Task<List<SatClaveProdServ>> Lista();
        Task<SatClaveProdServ> ObtenerPorClaveProdServ(string c_ClaveProdServ);
    }
}
