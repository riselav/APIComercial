using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatCajaServicio
    {
        Task<List<CatCaja>> Lista();
        Task<List<CatCaja>> ObtenerPorSucursal(long n_Sucursal);

        Task<CatCaja> ObtenerCajaPorId(long n_Caja);
    }
}