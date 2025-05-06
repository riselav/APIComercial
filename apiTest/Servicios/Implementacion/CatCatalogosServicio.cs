using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatCatalogosServicio : ICatCatalogosServicio
    {
        private readonly ICatCatalogosRepositorio _catalogosRepositorio;
        private readonly ILogger<CatCatalogosServicio> _logger;

        public CatCatalogosServicio(ILogger<CatCatalogosServicio> logger, ICatCatalogosRepositorio catalogosRepositorio)
        {
            _logger = logger;
            _catalogosRepositorio = catalogosRepositorio;
        }

        

        public async Task<List<CatCatalogos>> Lista()
        {
            try
            {
                return await _catalogosRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las catalogos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatCatalogos>> ObtenerPorNombre(string cNombre)
        {
            try
            {
                return await _catalogosRepositorio.ObtenerPorNombre(cNombre);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las catalogos")
                {
                    Metodo = "ObtenerPorNombre",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
