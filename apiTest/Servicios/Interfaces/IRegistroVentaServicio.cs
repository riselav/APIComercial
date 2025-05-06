using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegistroVentaServicio
    {
        Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta);
    }
}