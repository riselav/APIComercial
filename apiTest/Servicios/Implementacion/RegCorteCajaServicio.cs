using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class RegCorteCajaServicio: IRegCorteCajaServicio
    {
        private readonly IRegCorteCajaRepositorio _regCorteCajaRepositorio;
        private readonly ILogger<RegCorteCajaServicio> _logger;

        public RegCorteCajaServicio(ILogger<RegCorteCajaServicio> logger, IRegCorteCajaRepositorio regCorteCajaRepositorio)
        {
            _logger = logger;
            _regCorteCajaRepositorio = regCorteCajaRepositorio;
        }

        public async Task<RegCorteCaja> IME_REG_CorteCaja(RegCorteCaja regCorteCaja)
        {
            try
            {
                return await _regCorteCajaRepositorio.IME_REG_CorteCaja(regCorteCaja);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la corte caja")
                {
                    Metodo = "IME_REG_CorteCaja",
                    ErrorMessage = ex.Message,
                };
            }
        }

        
    }
}
