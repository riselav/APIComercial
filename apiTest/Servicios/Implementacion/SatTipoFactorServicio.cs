using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class SatTipoFactorServicio : ISatTipoFactorServicio
    {
        private readonly ISatTipoFactorRepositorio _SatTipoFactorRepositorio;
        private readonly ILogger<SatTipoFactorServicio> _logger;

        public SatTipoFactorServicio(ILogger<SatTipoFactorServicio> logger, ISatTipoFactorRepositorio SatTipoFactorRepositorio)
        {
            _logger = logger;
            _SatTipoFactorRepositorio = SatTipoFactorRepositorio;
        }

        public async Task<List<SatTipoFactor>> Lista()
        {
            try
            {
                return await _SatTipoFactorRepositorio.Lista();
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener las SatTipoFactor")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<SatTipoFactor> ObtenerPorTipoFactor(string c_TipoFactor)
        {
            try
            {
                return await _SatTipoFactorRepositorio.ObtenerPorTipoFactor(c_TipoFactor);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener las SatTipoFactor")
                {
                    Metodo = "ObtenerPorMarca",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
