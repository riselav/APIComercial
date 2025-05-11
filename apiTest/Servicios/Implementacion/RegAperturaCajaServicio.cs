using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class RegAperturaCajaServicio: IRegAperturaCajaServicio
    {
        private readonly IRegAperturaCajaRepositorio _regAperturaCajaRepositorio;
        private readonly ILogger<RegAperturaCajaServicio> _logger;

        public RegAperturaCajaServicio(ILogger<RegAperturaCajaServicio> logger, IRegAperturaCajaRepositorio regAperturaCajaRepositorio)
        {
            _logger = logger;
            _regAperturaCajaRepositorio = regAperturaCajaRepositorio;
        }

        public async Task<RegAperturaCaja> IME_REG_AperturaCaja(RegAperturaCaja regAperturaCaja)
        {
            try
            {
                return await _regAperturaCajaRepositorio.IME_REG_AperturaCaja(regAperturaCaja);
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
                    Metodo = "IME_REG_AperturaCaja",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
