using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class ArticulosVentaServicio : IArticuloVentaServicio
    {
        private readonly IArticulosVentaRepositorio _articulosVentaRepositorio;
        private readonly ILogger<ArticulosVentaServicio> _logger;

        public ArticulosVentaServicio(ILogger<ArticulosVentaServicio> logger, IArticulosVentaRepositorio articulosRepositorio)
        {
            _logger = logger;
            _articulosVentaRepositorio = articulosRepositorio;
        }
   
        public async Task<List<CatArticuloVenta>> ObtenArticulosVenta(ParametrosObtenArticulosVenta parametrosObtenArticulosVenta)
        {
            try
            {
                return await _articulosVentaRepositorio.ObtenArticulosVenta(parametrosObtenArticulosVenta);
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
                    Metodo = "ObtenArticulosVenta",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
