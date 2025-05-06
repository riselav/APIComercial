using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatProductosBaseServicio : ICatProductosBaseServicio
    {
        private readonly ICatProductosBaseRepositorio _productosBaseRepositorio;
        private readonly ILogger<CatProductosBaseServicio> _logger;

        public CatProductosBaseServicio(ILogger<CatProductosBaseServicio> logger, ICatProductosBaseRepositorio productosBaseRepositorio)
        {
            _logger = logger;
            _productosBaseRepositorio = productosBaseRepositorio;
        }

        public async Task<CatProductosBase> IME_CatProductosBase(CatProductosBase catProductoBase)
        {
            try
            {
                return await _productosBaseRepositorio.IME_CatProductosBase(catProductoBase);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la productoBase")
                {
                    Metodo = "IME_CatProductosBase",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatProductosBase>> Lista()
        {
            try
            {
                return await _productosBaseRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las productosBase")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatProductosBase> ObtenerPorProductoBase(int nProductoBase)
        {
            try
            {
                return await _productosBaseRepositorio.ObtenerPorProductoBase(nProductoBase);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las productosBase")
                {
                    Metodo = "ObtenerPorProductoBase",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
