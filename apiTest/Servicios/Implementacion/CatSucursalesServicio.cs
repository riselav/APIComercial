using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatSucursalesServicio: ICatSucursalesServicio
    {
        private readonly ICatSucursalesRepositorio _SucursalesRepositorio;
        private readonly ILogger<CatSucursalesServicio> _logger;

        public CatSucursalesServicio(ILogger<CatSucursalesServicio> logger, ICatSucursalesRepositorio SucursalesRepositorio)
        {
            _logger = logger;
            _SucursalesRepositorio = SucursalesRepositorio;
        }

        public async Task<CatSucursales> IME_CatSucursales(CatSucursales catSucursal)
        {
            try
            {
                return await _SucursalesRepositorio.IME_CatSucursales(catSucursal);
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

                _logger.LogError($"Error en {className}.{methodName} (CatSucursalesServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo guardar la sucursal")
                {
                    Metodo = "IME_CatSucursales",
                    ErrorMessage = ex.Message,
                };
            }
        }


        public async Task<List<CatSucursales>> Lista()
        {
            try
            {
                return await _SucursalesRepositorio.Lista();
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

                _logger.LogError($"Error en {className}.{methodName} (CatSucursalesServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener las sucursales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }


        public async Task<CatSucursales> ObtenerSucursal(int nSucursal)
        {
            try
            {
                return await _SucursalesRepositorio.ObtenerSucursal (nSucursal);
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

                _logger.LogError($"Error en {className}.{methodName} (CatSucursalesServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener la Sucursal")
                {
                    Metodo = "ObtenerSucursal",
                    ErrorMessage = ex.Message,
                };
            }
        }

    }
}
