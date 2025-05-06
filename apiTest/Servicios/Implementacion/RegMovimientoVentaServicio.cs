using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Interfaces;
using Voalaft.Data.Implementaciones;
using Voalaft.API.Exceptions;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;

namespace Voalaft.API.Servicios.Implementacion
{
    public class RegMovimientoVentaServicio:IRegistroVentaServicio
    {
        private readonly IRegMovimientoVentaRepositorio _registroVentaRepositorio;
        private readonly ILogger<RegMovimientoVentaServicio> _logger;

        public RegMovimientoVentaServicio(ILogger<RegMovimientoVentaServicio> logger, IRegMovimientoVentaRepositorio registroVentaRepositorio)
        {
            _logger = logger;
            _registroVentaRepositorio = registroVentaRepositorio;
        }

        public async Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta)
        {
            try
            {                
                return await _registroVentaRepositorio.IME_REG_VentasEncabezado(regMovimientoVenta);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la venta")
                {
                    Metodo = "RST_IME_REG_VentasEncabezado",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
