using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Dashboards.Indicadores;

namespace Voalaft.Data.Interfaces
{
    public interface IDashboardIndicadoresRepositorio
    {
        Task<DashboardIndicadores> ObtenerDashboardIndicadores(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);
        Task<SalesByCategoryDetail> ObtenerDetalleVentasPorCategoria(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<IncomeExpensesDetailDto> ObtenerDetalleIngresosVsGastos(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<InvoicedUninvoicedDetailDto> ObtenerDetalleFacturadoVsNoFacturado(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<PaymentMethodDetail> ObtenerDetalleVentaPorFormaPago(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<ServiceTypeDetail> ObtenerDetalleVentaPorTipoServicio(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<KitchenStationDetail> ObtenerDetalleVentaPorEstacionCocina(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);
    }
}