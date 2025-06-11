using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;

namespace Voalaft.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]

    public class RegistroCajaController : Controller
    {
        private readonly IRegAperturaCajaServicio _regAperturaCajaServicio;
        private readonly IRegMovimientoCajaServicio _regMovimientoCajaServicio;

        private readonly ILogger<RegistroCajaController> _logger;
        private readonly IConfiguration _config;

        public RegistroCajaController(ILogger<RegistroCajaController> logger, IConfiguration config,
                                    IRegAperturaCajaServicio regAperturaCajaServicio, IRegMovimientoCajaServicio regMovimientoCajaServicio)
        {
            _regAperturaCajaServicio = regAperturaCajaServicio;
            _regMovimientoCajaServicio = regMovimientoCajaServicio;

            _logger = logger;
            _config = config;
        }

        [HttpPost("IME_REG_AperturaCaja")]
        public async Task<ResultadoAPI> IME_REG_AperturaCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var regApertura = CryptographyUtils.DeserializarPeticion<RegAperturaCaja>(r);
                regApertura.Usuario = peticion.usuario;
                regApertura.Maquina = peticion.maquina;
                var regAperturaResult = await _regAperturaCajaServicio.IME_REG_AperturaCaja(regApertura);
                resultado = CryptographyUtils.CrearResultado(regAperturaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la apertura de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_REG_AperturaCaja2")]
        public async Task<RegAperturaCaja> IME_REG_AperturaCaja2([FromBody] RegAperturaCaja regApertura)
        {
            RegAperturaCaja resultado = null;
            try
            {
                regApertura.Usuario = "Web";
                regApertura.Maquina = "Comercial";
                var ventaResult = await _regAperturaCajaServicio.IME_REG_AperturaCaja(regApertura);
                resultado = ventaResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la apertura de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_REG_MovimientoCaja")]
        public async Task<ResultadoAPI> IME_REG_MovimientoCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var regMovimientoCaja = CryptographyUtils.DeserializarPeticion<RegMovimientoCaja>(r);
                regMovimientoCaja.Usuario = peticion.usuario;
                regMovimientoCaja.Maquina = peticion.maquina;
                var regMovimientoCajaResult = await _regMovimientoCajaServicio.IME_REG_MovimientoCaja(regMovimientoCaja);
                resultado = CryptographyUtils.CrearResultado(regMovimientoCajaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar el movimiento de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_REG_MovimientoCaja2")]
        public async Task<RegMovimientoCaja> IME_REG_MovimientoCaja2([FromBody] RegMovimientoCaja regMovimientoCaja)
        {
            RegMovimientoCaja resultado = null;
            try
            {
                resultado.Usuario = "Web";
                resultado.Maquina = "Comercial";
                var ventaResult = await _regMovimientoCajaServicio.IME_REG_MovimientoCaja(regMovimientoCaja);
                resultado = ventaResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar el movimiento de caja");
            }
            finally { }

            return resultado;
        }
                
        [HttpPost("ObtenAperturaAbierta")]
        public async Task<ResultadoAPI> ObtenAperturaAbierta(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var apertura = CryptographyUtils.DeserializarPeticion<RegAperturaCaja>(r);
                RegAperturaCaja aperturaCajaAbierta = await _regAperturaCajaServicio.ObtenAperturaAbierta(apertura);
                resultado = CryptographyUtils.CrearResultado(aperturaCajaAbierta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat turnos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenMovimientosCaja")]
        public async Task<ResultadoAPI> ObtenMovimientosCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaCajas = CryptographyUtils.DeserializarPeticion<ParametrosConsultaMovimientosCaja>(r);
                List<RegMovimientoCaja> movimientos = await _regMovimientoCajaServicio.ObtenMovimientosCaja(parametrosConsultaCajas);
                resultado = CryptographyUtils.CrearResultado(movimientos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de movimientos de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenImporteDisponibleCaja")]
        public async Task<ResultadoAPI> ObtenImporteDisponibleCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaCajas = CryptographyUtils.DeserializarPeticion<ParametrosConsultaMovimientosCaja>(r);
                Decimal disponible = await _regMovimientoCajaServicio.ObtenImporteDisponibleCaja (parametrosConsultaCajas);
                resultado = CryptographyUtils.CrearResultado(disponible);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el disponible de caja");
            }
            finally { }

            return resultado;
        }
    }
}
