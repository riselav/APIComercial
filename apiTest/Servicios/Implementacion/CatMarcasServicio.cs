using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatMarcasServicio : ICatMarcasServicio
    {
        private readonly ICatMarcasRepositorio _MarcasRepositorio;
        private readonly ILogger<CatMarcasServicio> _logger;

        public CatMarcasServicio(ILogger<CatMarcasServicio> logger, ICatMarcasRepositorio MarcasRepositorio)
        {
            _logger = logger;
            _MarcasRepositorio = MarcasRepositorio;
        }

        public async Task<CatMarcas> IME_CatMarcas(CatMarcas catMarca)
        {
            try
            {
                return await _MarcasRepositorio.IME_CatMarcas(catMarca);
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
                    Metodo = "IME_CatMarcas",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatMarcas>> Lista()
        {
            try
            {
                return await _MarcasRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las Marcas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatMarcas> ObtenerPorMarca(int nMarca)
        {
            try
            {
                return await _MarcasRepositorio.ObtenerPorMarca(nMarca);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las Marcas")
                {
                    Metodo = "ObtenerPorMarca",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
