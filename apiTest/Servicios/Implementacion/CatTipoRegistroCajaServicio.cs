using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatTipoRegistroCajaServicio: ICatTipoRegistroCajaServicio
    {
        private readonly ICatTipoRegistroCajaRepositorio _catTipoRegistroCajaRepositorio;
        private readonly ILogger<CatTipoRegistroCajaServicio> _logger;

        public CatTipoRegistroCajaServicio(ILogger<CatTipoRegistroCajaServicio> logger, ICatTipoRegistroCajaRepositorio catTipoRegistroCajaRepositorio)
        {
            _logger = logger;
            _catTipoRegistroCajaRepositorio = catTipoRegistroCajaRepositorio;
        }

        public async Task<List<CatTipoRegistroCaja>> Lista()
        {
            try
            {
                return await _catTipoRegistroCajaRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener la lista de tipo de registros de caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatTipoRegistroCaja> ObtenerPorId(int n_TipoRegistroCaja)
        {
            try
            {
                return await _catTipoRegistroCajaRepositorio.ObtenerPorId(n_TipoRegistroCaja);
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
                throw new ServiciosException("Error(srv) No se pudo obtener tipo de registro de caja")
                {
                    Metodo = "ObtenerPorId",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
