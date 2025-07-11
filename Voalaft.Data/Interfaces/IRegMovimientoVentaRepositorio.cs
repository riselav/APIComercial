using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Consultas;

namespace Voalaft.Data.Interfaces
{
    public interface IRegMovimientoVentaRepositorio
    {
        Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta);
        Task<List<MovimientoVentaEnc>> CM_CON_Todas_Cotizaciones(int nSucursal);
        Task<List<MovimientoVentaDet>> CM_CON_detalle_mov_ventas(long nVenta);
    }
}