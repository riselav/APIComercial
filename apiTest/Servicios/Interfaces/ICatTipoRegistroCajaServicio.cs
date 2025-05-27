using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatTipoRegistroCajaServicio
    {
        Task<List<CatTipoRegistroCaja>> Lista();
        Task<CatTipoRegistroCaja> ObtenerPorId(int n_TipoRegistroCaja);
    }
}