using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatalagosSATServicio : ICatalagosSATServicio
    {
        private readonly ICatalagosSATRepositorio _catalagosSATRepositorio;
        private readonly ILogger<CatalagosSATServicio> _logger;

        public CatalagosSATServicio(ILogger<CatalagosSATServicio> logger, ICatalagosSATRepositorio catalagosSATRepositorio)
        {
            _logger = logger;
            _catalagosSATRepositorio = catalagosSATRepositorio;
        }

        public async Task<List<CatUnidadesSat>> ListaCatUnidades()
        {
            try
            {
                return await _catalagosSATRepositorio.ListaCatUnidades();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidades SAT")
                {
                    Metodo = "ListaCatUnidades",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatUnidadesSat> ObtenerUnidadesPorClave(string cClave)
        {
            try
            {
                return await _catalagosSATRepositorio.ObtenerUnidadesPorClave(cClave);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidades SAT")
                {
                    Metodo = "ObtenerUnidadesPorClave",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
