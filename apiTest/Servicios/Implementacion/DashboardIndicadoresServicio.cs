using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Dashboards.Indicadores;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class DashboardIndicadoresServicio : IDashboardIndicadoresServicio
    {
        private readonly IDashboardIndicadoresRepositorio _dashboardIndicadoresRepositorio;
        private readonly ILogger<DashboardIndicadoresServicio> _logger;

        public DashboardIndicadoresServicio(ILogger<DashboardIndicadoresServicio> logger, IDashboardIndicadoresRepositorio dashboardIndicadoresRepositorio)
        {
            _logger = logger;
            _dashboardIndicadoresRepositorio = dashboardIndicadoresRepositorio;
        }

        public async Task<DashboardIndicadores> ObtenerDashboardIndicadores(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            try
            {
                return await _dashboardIndicadoresRepositorio.ObtenerDashboardIndicadores(n_Sucursal, n_FechaInicial, n_FechaFinal);
            }
            catch (DataAccessException ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);
                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener dashboard indicadores")
                {
                    Metodo = "ObtenerDashboardIndicadores",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
