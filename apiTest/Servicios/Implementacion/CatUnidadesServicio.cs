using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatUnidadesServicio : ICatUnidadesServicio
    {
        private readonly ICatUnidadesRepositorio _unidadesRepositorio;
        private readonly ILogger<CatUnidadesServicio> _logger;

        public CatUnidadesServicio(ILogger<CatUnidadesServicio> logger, ICatUnidadesRepositorio unidadesRepositorio)
        {
            _logger = logger;
            _unidadesRepositorio = unidadesRepositorio;
        }

        public async Task<CatUnidades> IME_CatUnidades(CatUnidades catUnidad)
        {
            try
            {
                return await _unidadesRepositorio.IME_CatUnidades(catUnidad);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la subliena")
                {
                    Metodo = "IME_CatUnidades",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatUnidades>> Lista()
        {
            try
            {
                return await _unidadesRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidades")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatUnidades> ObtenerPorUnidad(int nUnidad)
        {
            try
            {
                return await _unidadesRepositorio.ObtenerPorUnidad(nUnidad);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las unidades")
                {
                    Metodo = "ObtenerPorUnidad",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
