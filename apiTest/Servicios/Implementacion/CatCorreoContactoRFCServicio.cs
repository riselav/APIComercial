using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatCorreoContactoRFCServicio: ICatCorreoContactoRFCServicio
    {
        private readonly ICatCorreoContactoRFCRepositorio _catCorreoContactoRFCRepositorio;
        private readonly ILogger<CatCorreoContactoRFCServicio> _logger;

        public CatCorreoContactoRFCServicio(ILogger<CatCorreoContactoRFCServicio> logger, ICatCorreoContactoRFCRepositorio catCorreoContactoRFCRepositorio)
        {
            _logger = logger;
            _catCorreoContactoRFCRepositorio = catCorreoContactoRFCRepositorio;
        }

        public async Task<List<CatCorreoContactoRFC>> ObtenerPorId(long n_IDRFC)
        {
            try
            {
                return await _catCorreoContactoRFCRepositorio.ObtenerPorId(n_IDRFC);
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
                throw new ServiciosException("Error(srv) No se pudo obtener los correo de contacto rfc")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
