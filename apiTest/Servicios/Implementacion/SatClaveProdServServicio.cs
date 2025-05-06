using System.Reflection;
using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class SatClaveProdServServicio : ISatClaveProdServServicio
    {
        private readonly ISatClaveProdServRepositorio _SatClaveProdServRepositorio;
        private readonly ILogger<SatClaveProdServServicio> _logger;

        public SatClaveProdServServicio(ILogger<SatClaveProdServServicio> logger, ISatClaveProdServRepositorio SatClaveProdServRepositorio)
        {
            _logger = logger;
            _SatClaveProdServRepositorio = SatClaveProdServRepositorio;
        }

        public async Task<List<SatClaveProdServ>> Lista()
        {
            try
            {
                return await _SatClaveProdServRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las SatClaveProdServ")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<SatClaveProdServ> ObtenerPorClaveProdServ(string cValor)
        {
            try
            {
                return await _SatClaveProdServRepositorio.ObtenerPorClaveProdServ(cValor);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las SatClaveProdServ")
                {
                    Metodo = "ObtenerPorClaveProdServ",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
