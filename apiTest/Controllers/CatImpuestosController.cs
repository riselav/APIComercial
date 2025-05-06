using Voalaft.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Microsoft.AspNetCore.Authorization;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatImpuestosController : ControllerBase
    {

        private readonly ICatImpuestosServicio _impuestosServicio;
        private readonly ILogger<CatImpuestosController> _logger;
        private readonly IConfiguration _config;

        public CatImpuestosController(ILogger<CatImpuestosController> logger, IConfiguration config,ICatImpuestosServicio impuestosServicio)
        {
            _impuestosServicio = impuestosServicio;
            _logger = logger;
            _config = config;
        }

        [HttpPost("Lista")]
        public async Task<ResultadoAPI> Lista(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);                
                List<CatImpuestos> impuestos = await _impuestosServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(impuestos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las impuestos");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorImpuesto")]
        public async Task<ResultadoAPI> ObtenerPorImpuesto(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var impuesto = CryptographyUtils.DeserializarPeticion<CatImpuestos>(r);
                var impuestoResult = await _impuestosServicio.ObtenerPorImpuesto(impuesto.Impuesto);
                resultado = CryptographyUtils.CrearResultado(impuestoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las impuestos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatImpuestos")]
        public async Task<ResultadoAPI> IME_CatImpuestos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var impuesto = CryptographyUtils.DeserializarPeticion<CatImpuestos>(r);
                impuesto.Usuario = peticion.usuario;
                impuesto.Maquina = peticion.maquina;
                var impuestoResult = await _impuestosServicio.IME_CatImpuestos(impuesto);
                resultado = CryptographyUtils.CrearResultado(impuestoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la impuesto");
            }
            finally { }

            return resultado;
        }
    }
}
