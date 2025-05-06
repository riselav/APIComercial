using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatImpuestosServicio : ICatImpuestosServicio
    {
        private readonly ICatImpuestosRepositorio _impuestosRepositorio;
        private readonly ILogger<CatImpuestosServicio> _logger;

        public CatImpuestosServicio(ILogger<CatImpuestosServicio> logger, ICatImpuestosRepositorio impuestosRepositorio)
        {
            _logger = logger;
            _impuestosRepositorio = impuestosRepositorio;
        }

        public async Task<CatImpuestos> IME_CatImpuestos(CatImpuestos catImpuesto)
        {
            try
            {
                return await _impuestosRepositorio.IME_CatImpuestos(catImpuesto);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la impuesto")
                {
                    Metodo = "IME_CatImpuestos",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatImpuestos>> Lista()
        {
            try
            {
                return await _impuestosRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las impuestos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatImpuestos> ObtenerPorImpuesto(int nImpuesto)
        {
            try
            {
                return await _impuestosRepositorio.ObtenerPorImpuesto(nImpuesto);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las impuestos")
                {
                    Metodo = "ObtenerPorImpuesto",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
