
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatTipoFactorRepositorio
    {
        Task<List<SatTipoFactor>> Lista();
        Task<SatTipoFactor> ObtenerPorTipoFactor(string c_TipoFactor);
    }
}
