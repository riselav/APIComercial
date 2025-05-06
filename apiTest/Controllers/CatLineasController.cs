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
    public class CatLineasController : ControllerBase
    {

        private readonly ICatLineassServicio _lineasService;
        private readonly ILogger<CatLineasController> _logger;
        private readonly IConfiguration _config;

        public CatLineasController(ILogger<CatLineasController> logger, IConfiguration config,ICatLineassServicio lineasService)
        {
            _lineasService = lineasService;
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
                List<CatLineas> lineas = await _lineasService.Lista();                
                resultado = CryptographyUtils.CrearResultado(lineas);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las lineas");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorLinea")]
        public async Task<ResultadoAPI> ObtenerPorLinea(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatLineas>(r);
                var lineaResult = await _lineasService.ObtenerPorLinea(linea.Linea);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las lineas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatLineas")]
        public async Task<ResultadoAPI> IME_CatLineas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatLineas>(r);
                linea.Usuario = peticion.usuario;
                linea.Maquina = peticion.maquina;
                var lineaResult = await _lineasService.IME_CatLineas(linea);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la linea");
            }
            finally { }

            return resultado;
        }
    }
}
