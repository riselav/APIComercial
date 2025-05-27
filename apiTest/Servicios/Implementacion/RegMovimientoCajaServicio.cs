using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class RegMovimientoCajaServicio : IRegMovimientoCajaServicio
    {
        private readonly IRegMovimientoCajaRepositorio _regMovimientoCajaRepositorio;
        private readonly ILogger<RegMovimientoCajaServicio> _logger;

        public RegMovimientoCajaServicio(ILogger<RegMovimientoCajaServicio> logger, IRegMovimientoCajaRepositorio regMovimientoCajaRepositorio)
        {
            _logger = logger;
            _regMovimientoCajaRepositorio = regMovimientoCajaRepositorio;
        }

        public async Task<RegMovimientoCaja> IME_REG_MovimientoCaja(RegMovimientoCaja regMovimientoCaja)
        {
            try
            {
                return await _regMovimientoCajaRepositorio.IME_REG_MovimientoCaja (regMovimientoCaja);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la apertura caja")
                {
                    Metodo = "IME_REG_MovimientoCaja",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
