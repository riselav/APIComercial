using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Consultas;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegistroVentaServicio
    {
        Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta);
        Task<List<MovimientoVentaEnc>> CM_CON_Todas_Cotizaciones(int nSucursal);
        Task<List<MovimientoVentaDet>> CM_CON_detalle_mov_ventas(long nVenta);
    }
}