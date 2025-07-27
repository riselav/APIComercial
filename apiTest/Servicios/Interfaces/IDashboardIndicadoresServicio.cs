using Voalaft.Data.Entidades.Dashboards.Indicadores;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IDashboardIndicadoresServicio
    {
        Task<DashboardIndicadores> ObtenerDashboardIndicadores(int n_Sucursal, int n_FechaInicial, int n_FechaFinal);
    }
}