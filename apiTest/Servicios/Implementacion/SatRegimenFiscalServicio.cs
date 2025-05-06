using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class SatRegimenFiscalServicio: ISatRegimenFiscalServicio
    {
        private readonly ISatRegimenFiscalRepositorio _SatRegimenFiscalRepositorio;
        private readonly ILogger<SatRegimenFiscalServicio> _logger;

        public SatRegimenFiscalServicio(ILogger<SatRegimenFiscalServicio> logger, ISatRegimenFiscalRepositorio SatRegimenFiscalRepositorio)
        {
            _logger = logger;
            _SatRegimenFiscalRepositorio = SatRegimenFiscalRepositorio;
        }

        public async Task<List<SatRegimenFiscal>> Lista()
        {
            try
            {
                return await _SatRegimenFiscalRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las SatTasaOCuota")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<SatRegimenFiscal> ObtenerPorId(long n_RegimenFiscal)
        {
            try
            {
                return await _SatRegimenFiscalRepositorio.ObtenerPorId(n_RegimenFiscal);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las SatTasaOCuota")
                {
                    Metodo = "ObtenerPorMarca",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
