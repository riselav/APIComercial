using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatFormaPagoServicio
    {
        Task<List<CatFormaPago>> Lista();
        Task<CatFormaPago> ObtenerPorId(long n_FormaPago);

        Task<List<CatFormaPago>> ObtenerPorTipoEgreso(int n_TipoEgreso);
    }
}
