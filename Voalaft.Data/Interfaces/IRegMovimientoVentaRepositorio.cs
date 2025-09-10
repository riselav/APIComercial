using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Consultas;

namespace Voalaft.Data.Interfaces
{
    public interface IRegMovimientoVentaRepositorio
    {
        Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta);
        Task<List<MovimientoVentaEnc>> CM_CON_Todas_Cotizaciones(int nSucursal);
        Task<List<MovimientoVentaDet>> CM_CON_detalle_mov_ventas(long nVenta);
        Task<ImpresionTicketData> Obtener_Ticket_Venta(long nVenta);
        Task<List<ReporteVentas>> CM_CON_reporte_ventas_sp(ParametrosReporteVentas parametrosReporteVentas);
        Task<ParamCancelaVenta> IME_CAN_Cancelar_Venta(ParamCancelaVenta paramCancelaVenta);
        Task<List<FormasPagoImporte>> CM_CON_FormasPago_Venta(long nVenta);
    }
}