using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatDatosDireccionServicio : ICatDatosDireccionServicio
    {
        private readonly ICatDatosDireccionRepositorio _datosDireccionRepositorio;
        private readonly ILogger<CatDatosDireccionServicio> _logger;

        public CatDatosDireccionServicio(ILogger<CatDatosDireccionServicio> logger, ICatDatosDireccionRepositorio datosDireccionRepositorio)
        {
            _logger = logger;
            _datosDireccionRepositorio = datosDireccionRepositorio;
        }        

        public async Task<List<CatCodigosPostales>> ListaCodigosPostales()
        {
            try
            {
                return await _datosDireccionRepositorio.ListaCodigosPostales();
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
                throw new ServiciosException("Error(srv) No se pudo obtener los Codigos postales")
                {
                    Metodo = "ListaCodigosPostales",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatColonias>> ObtenerColoniasPorCP(string cCodigoPostal)
        {
            try
            {
                return await _datosDireccionRepositorio.ObtenerColoniasPorCP(cCodigoPostal);
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
                throw new ServiciosException("Error(srv) No se pudo obtener los Codigos postales")
                {
                    Metodo = "ObtenerPorCP",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<DireccionPorCodigoPostal> DireccionPorCodigoPostal(string cCodigoPostal)
        {
            try
            {
                return await _datosDireccionRepositorio.DireccionPorCodigoPostal(cCodigoPostal);
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
                throw new ServiciosException("Error(srv) No se pudo obtener los datos de dirección del  Codigo postal")
                {
                    Metodo = "DireccionPorCodigoPostal",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
