using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatUnidadesRelacionalesServicio : ICatUnidadesRelacionalesServicio
    {
        private readonly ICatUnidadesRelacionalesRepositorio _unidadesRelacionalesRepositorio;
        private readonly ILogger<CatUnidadesRelacionalesServicio> _logger;

        public CatUnidadesRelacionalesServicio(ILogger<CatUnidadesRelacionalesServicio> logger, ICatUnidadesRelacionalesRepositorio unidadesRelacionalesRepositorio)
        {
            _logger = logger;
            _unidadesRelacionalesRepositorio = unidadesRelacionalesRepositorio;
        }

        public async Task<CatUnidadesRelacionales> IME_CatUnidadesRelacionales(CatUnidadesRelacionales catUnidadRelacional)
        {
            try
            {
                return await _unidadesRelacionalesRepositorio.IME_CatUnidadesRelacionales(catUnidadRelacional);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la unidadRelacional")
                {
                    Metodo = "IME_CatUnidadesRelacionales",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatUnidadesRelacionales>> Lista()
        {
            try
            {
                return await _unidadesRelacionalesRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidadesRelacionales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatUnidadesRelacionales> ObtenerPorUnidadRelacional(int nUnidadRelacional)
        {
            try
            {
                return await _unidadesRelacionalesRepositorio.ObtenerPorUnidadRelacional(nUnidadRelacional);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidadesRelacionales")
                {
                    Metodo = "ObtenerPorUnidadRelacional",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
