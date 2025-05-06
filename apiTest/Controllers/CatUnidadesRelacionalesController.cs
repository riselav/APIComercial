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
    public class CatUnidadesRelacionalesController : ControllerBase
    {

        private readonly ICatUnidadesRelacionalesServicio _unidadesRelacionalesServicio;
        private readonly ILogger<CatUnidadesRelacionalesController> _logger;
        private readonly IConfiguration _config;

        public CatUnidadesRelacionalesController(ILogger<CatUnidadesRelacionalesController> logger, IConfiguration config,ICatUnidadesRelacionalesServicio unidadesRelacionalesServicio)
        {
            _unidadesRelacionalesServicio = unidadesRelacionalesServicio;
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
                List<CatUnidadesRelacionales> unidadesRelacionales = await _unidadesRelacionalesServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(unidadesRelacionales);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las unidadesRelacionales");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorUnidadRelacional")]
        public async Task<ResultadoAPI> ObtenerPorUnidadRelacional(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var unidadRelacional = CryptographyUtils.DeserializarPeticion<CatUnidadesRelacionales>(r);
                var unidadRelacionalResult = await _unidadesRelacionalesServicio.ObtenerPorUnidadRelacional(unidadRelacional.UnidadRelacional);
                resultado = CryptographyUtils.CrearResultado(unidadRelacionalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las unidadesRelacionales");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatUnidadesRelacionales")]
        public async Task<ResultadoAPI> IME_CatUnidadesRelacionales(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var unidadRelacional = CryptographyUtils.DeserializarPeticion<CatUnidadesRelacionales>(r);
                unidadRelacional.Usuario = peticion.usuario;
                unidadRelacional.Maquina = peticion.maquina;
                var unidadRelacionalResult = await _unidadesRelacionalesServicio.IME_CatUnidadesRelacionales(unidadRelacional);
                resultado = CryptographyUtils.CrearResultado(unidadRelacionalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la unidadRelacional");
            }
            finally { }

            return resultado;
        }
    }
}
