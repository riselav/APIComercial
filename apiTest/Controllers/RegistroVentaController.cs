using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Consultas;
using Voalaft.Utilerias;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class RegistroVentaController : ControllerBase
    {
        private readonly IRegistroVentaServicio _registroVentaServicio;
        private readonly ILogger<RegistroVentaController> _logger;
        private readonly IConfiguration _config;

        public RegistroVentaController(ILogger<RegistroVentaController> logger, IConfiguration config, IRegistroVentaServicio registroVentaServicio)
        {
            _registroVentaServicio = registroVentaServicio;
            _logger = logger;
            _config = config;
        }

        [HttpPost("IME_REG_VentasEncabezado")]
        public async Task<ResultadoAPI> IME_REG_VentasEncabezado(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var regVenta = CryptographyUtils.DeserializarPeticion<RegMovimientoVenta>(r);
                regVenta.Usuario = peticion.usuario;
                regVenta.Maquina = peticion.maquina;
                var ventaResult = await _registroVentaServicio.IME_REG_VentasEncabezado(regVenta);
                resultado = CryptographyUtils.CrearResultado(ventaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la venta");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_REG_VentasEncabezado2")]
        public async Task<RegMovimientoVenta> IME_REG_VentasEncabezado([FromBody] RegMovimientoVenta regMovimientoVenta)
        {
            RegMovimientoVenta resultado = null;
            try
            {
               regMovimientoVenta.Usuario = "Web";  
                regMovimientoVenta.Maquina = "Comercial";
                var ventaResult = await _registroVentaServicio.IME_REG_VentasEncabezado(regMovimientoVenta);
                resultado = ventaResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la venta");
            }
            finally { }

            return resultado;
        }

        [HttpPost("CM_CON_Todas_Cotizaciones")]
        public async Task<ResultadoAPI> CM_CON_Todas_Cotizaciones(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var mv = CryptographyUtils.DeserializarPeticion<RegMovimientoVenta>(r);
                List<MovimientoVentaEnc> cotizaciones = await _registroVentaServicio.CM_CON_Todas_Cotizaciones(mv.nSucursal);
                resultado = CryptographyUtils.CrearResultado(cotizaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cotizaciones");
            }
            finally { }

            return resultado;
        }

        [HttpPost("CM_CON_detalle_mov_ventas")]
        public async Task<ResultadoAPI> CM_CON_detalle_mov_ventas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var mv = CryptographyUtils.DeserializarPeticion<RegMovimientoVenta>(r);
                List<MovimientoVentaDet> detalle = await _registroVentaServicio.CM_CON_detalle_mov_ventas(mv.nVenta);
                resultado = CryptographyUtils.CrearResultado(detalle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de detalles de venta");
            }
            finally { }

            return resultado;
        }
    }
}
