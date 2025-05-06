using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatAlmacenesServicio: ICatAlmacenesServicio
    {

        private readonly ICatAlmacenesRepositorio  _AlmacenesRepositorio;
        private readonly ILogger<CatAlmacenesServicio > _logger;

        public CatAlmacenesServicio(ILogger<CatAlmacenesServicio > logger, ICatAlmacenesRepositorio AlmacenesRepositorio)
        {
            _logger = logger;
            _AlmacenesRepositorio = AlmacenesRepositorio;
        }

        public async Task<CatAlmacenes > IME_CatAlmacenes(CatAlmacenes  objAlmacen)
        {
            try
            {
                return await _AlmacenesRepositorio.IME_CatAlmacenes(objAlmacen);
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

                _logger.LogError($"Error en {className}.{methodName} (IME_CatLineas {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al guardar el almacén")
                {
                    Metodo = "IME_CatAlmacenes",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatAlmacenes>> Lista(int nSucursal, int nPlaza, string CodigoPostal, string Descripcion)
        {
            try
            {
                return []; //;await _AlmacenesRepositorio.Lista( nSucursal,  nPlaza,  CodigoPostal,  Descripcion);
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

                _logger.LogError($"Error en {className}.{methodName} (Lista {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al obtener los almacenes")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }


        public async Task<CatAlmacenes> ObtenerAlmacen(int nAlmacen)
        {
            try
            {
                return await _AlmacenesRepositorio.ObtenerAlmacen(nAlmacen);
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

                _logger.LogError($"Error en {className}.{methodName} (ObtenerAlmacen {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al obtener la linea")
                {
                    Metodo = "ObtenerAlmacen",
                    ErrorMessage = ex.Message,
                };
            }
        }


    }
}
