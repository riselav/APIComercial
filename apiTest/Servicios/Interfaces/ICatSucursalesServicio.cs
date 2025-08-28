using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatSucursalesServicio
    {
        Task<List<CatSucursales >> Lista();

        Task<CatSucursales> ObtenerSucursal(int nSucursal);

        Task<CatSucursales> IME_CatSucursales(CatSucursales catSucursal);

        Task<CatSucursales> IME_ConfiguracionTicketSucursal(CatSucursales catSucursal);

        Task<CatConfiguracionTicketSucursal> ObtenerConfiguracionTicketSucursal(int nSucursal);
    }
}
