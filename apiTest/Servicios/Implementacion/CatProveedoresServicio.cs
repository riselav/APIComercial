using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatProveedoresServicio : ICatProveedoresServicio
    {
        private readonly ICatProveedoresRepositorio _ProveedoresRepositorio;
        private readonly ILogger<CatProveedoresServicio> _logger;

        public CatProveedoresServicio (ILogger<CatProveedoresServicio > logger, ICatProveedoresRepositorio  proveedoresRepositorio )
        {
            _logger = logger;
            _ProveedoresRepositorio = proveedoresRepositorio;
        }

        public async Task<CatProveedores> IME_CatProveedores(CatProveedores objProveedor)
        {
            try
            {
                return await _ProveedoresRepositorio.IME_CatProveedores(objProveedor);
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
                    Metodo = "CAT_IME_Proveedores",
                    ErrorMessage = ex.Message,
                };
            }
        }


        public async Task<List<CatProveedores >> Lista()
        {
            try
            {
                return await _ProveedoresRepositorio.Lista ();
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
                throw new ServiciosException("Error(srv) No se pudo obtener información del proveedor")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }


        public async Task<CatProveedores > ObtenerProveedor(int nProveedor)
        {
            try
            {
                return await _ProveedoresRepositorio.ObtenerProveedor (nProveedor);
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
                throw new ServiciosException("Error(srv) No se pudo obtener información del proveedor")
                {
                    Metodo = "ObtenerProveedor",
                    ErrorMessage = ex.Message,
                };
            }
        }


    }
}
