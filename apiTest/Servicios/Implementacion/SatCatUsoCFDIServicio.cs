using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class SatCatUsoCFDIServicio : ISatCatUsoCFDIServicio
    {
        private readonly ISatCatUsoCFDIRepositorio _SatCatUsoCFDIRepositorio;
        private readonly ILogger<SatCatUsoCFDIServicio> _logger;


        public SatCatUsoCFDIServicio(ILogger<SatCatUsoCFDIServicio> logger, ISatCatUsoCFDIRepositorio satCatUsoCFDIRepositorio)
        {
            _logger = logger;
            _SatCatUsoCFDIRepositorio = satCatUsoCFDIRepositorio;
        }

        public async Task<List<SatCatUsoCFDI>> Lista()
        {
            try
            {
                return await _SatCatUsoCFDIRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener la lista de Uso CFDI")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<SatCatUsoCFDI> ObtenerPorClave(string c_UsoCFDI)
        {
            try
            {
                return await _SatCatUsoCFDIRepositorio.ObtenerPorClave(c_UsoCFDI);
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
                throw new ServiciosException("Error(srv) No se pudo obtener el Uso CFDI")
                {
                    Metodo = "ObtenerPorMarca",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}