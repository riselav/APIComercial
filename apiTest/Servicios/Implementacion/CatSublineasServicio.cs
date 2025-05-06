using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatSublineasServicio : ICatSublineasServicio
    {
        private readonly ICatSublineasRepositorio _sublineasRepositorio;
        private readonly ILogger<CatSublineasServicio> _logger;

        public CatSublineasServicio(ILogger<CatSublineasServicio> logger, ICatSublineasRepositorio lineasRepositorio)
        {
            _logger = logger;
            _sublineasRepositorio = lineasRepositorio;
        }

        public async Task<CatSublineas> IME_CatSublineas(CatSublineas catSublinea)
        {
            try
            {
                return await _sublineasRepositorio.IME_CatSublineas(catSublinea);
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
                    Metodo = "IME_CatSublineas",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatSublineas>> Lista()
        {
            try
            {
                return await _sublineasRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las sublineas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatSublineas> ObtenerPorSublinea(int nSublinea)
        {
            try
            {
                return await _sublineasRepositorio.ObtenerPorSublinea(nSublinea);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las sublineas")
                {
                    Metodo = "ObtenerPorSublinea",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
