using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatTipoFactorServicio
    {
        Task<List<SatTipoFactor>> Lista();
        Task<SatTipoFactor> ObtenerPorTipoFactor(string c_TipoFactor);
    }
}
