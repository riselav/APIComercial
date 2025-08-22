using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatPlazasServicio : ICatPlazasServicio
    {
        private readonly ICatPlazasRepositorio _plazasRepositorio;
        private readonly ILogger<CatPlazasServicio> _logger;

        public CatPlazasServicio(ILogger<CatPlazasServicio> logger, ICatPlazasRepositorio plazasRepositorio)
        {
            _logger = logger;
            _plazasRepositorio = plazasRepositorio;
        }

        public async Task<List<CatPlaza>> Lista()
        {
            try
            {
                return await _plazasRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las plazas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatPlaza> ObtenerPorPlaza(int nPlaza)
        {
            try
            {
                return await _plazasRepositorio.ObtenerPorPlaza(nPlaza);
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
                throw new ServiciosException("Error(srv) No se pudo obtener la plaza")
                {
                    Metodo = "ObtenerPorPlaza",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}