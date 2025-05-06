using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatArticulosServicio : ICatArticulosServicio
    {
        private readonly ICatArticulosRepositorio _articulosRepositorio;
        private readonly ICatProductosBaseRepositorio _catProductosBaseRepositorio;

        private readonly ILogger<CatArticulosServicio> _logger;

        public CatArticulosServicio(ILogger<CatArticulosServicio> logger, ICatArticulosRepositorio articulosRepositorio, ICatProductosBaseRepositorio catProductosBaseRepositorio)
        {
            _logger = logger;
            _articulosRepositorio = articulosRepositorio;
            _catProductosBaseRepositorio = catProductosBaseRepositorio;
        }

        public async Task<List<ConsultaArticulo>> ConsultaTableroArticulos(ParametrosConsultaArticulo parametrosConsultaArticulo)
        {
            try
            {
                return await _articulosRepositorio.ConsultaTableroArticulos(parametrosConsultaArticulo);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las articulos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatArticulos> IME_CatArticulos(CatArticulos catArticulo)
        {
            try
            {
                if(catArticulo.bInsumoFinal)
                {
                    var pb = new CatProductosBase
                    {
                        ProductoBase = 0,
                        Descripcion = catArticulo.Descripcion,
                        TipoUnidad = catArticulo.TipoUnidad == null ? (short)1 : (short)catArticulo.TipoUnidad,
                        Activo=true,
                        Maquina=catArticulo.Maquina,
                        Usuario=catArticulo.Usuario,
                    };

                    pb= await _catProductosBaseRepositorio.IME_CatProductosBase(pb);
                    catArticulo.IdProductoBase = pb.ProductoBase;
                }
                return await _articulosRepositorio.IME_CatArticulos(catArticulo);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la articulo")
                {
                    Metodo = "IME_CatArticulos",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatArticulos>> Lista()
        {
            try
            {
                return await _articulosRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las articulos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatArticulos> ObtenerPorArticulo(int nArticulo)
        {
            try
            {
                return await _articulosRepositorio.ObtenerPorArticulo(nArticulo);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las articulos")
                {
                    Metodo = "ObtenerPorArticulo",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<SucursalesCombo>> ListaSucursales()
        {
            try
            {
                return await _articulosRepositorio.ListaSucursales();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las sucursales")
                {
                    Metodo = "ListaSucursales",
                    ErrorMessage = ex.Message,
                };
            }
        }

    }
}
