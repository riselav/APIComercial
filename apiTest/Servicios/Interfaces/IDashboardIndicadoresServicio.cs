using Voalaft.Data.Entidades.Dashboards.Indicadores;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IDashboardIndicadoresServicio
    {
        Task<DashboardIndicadores> ObtenerDashboardIndicadores(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<SalesByCategoryDetail> ObtenerDetalleVentasPorCategoria(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);

        Task<IncomeExpensesDetailDto> ObtenerDetalleIngresosVsGastos(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);
    }
}