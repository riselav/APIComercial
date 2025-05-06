using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatLineassServicio : ICatLineassServicio
    {
        private readonly ICatLineasRepositorio _lineasRepositorio;
        private readonly ILogger<CatLineassServicio> _logger;

        public CatLineassServicio(ILogger<CatLineassServicio> logger, ICatLineasRepositorio lineasRepositorio)
        {
            _logger = logger;
            _lineasRepositorio = lineasRepositorio;
        }

        public async Task<CatLineas> IME_CatLineas(CatLineas catLinea)
        {
            try
            {
                return await _lineasRepositorio.IME_CatLineas(catLinea);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la liena")
                {
                    Metodo = "IME_CatLineas",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatLineas>> Lista()
        {
            try
            {
                return await _lineasRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las lineas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatLineas> ObtenerPorLinea(int nLinea)
        {
            try
            {
                return await _lineasRepositorio.ObtenerPorLinea(nLinea);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las lineas")
                {
                    Metodo = "ObtenerPorLinea",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
