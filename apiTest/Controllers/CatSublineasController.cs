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
    public class CatSublineasController : ControllerBase
    {

        private readonly ICatSublineasServicio _sublineasService;
        private readonly ILogger<CatSublineasController> _logger;
        private readonly IConfiguration _config;

        public CatSublineasController(ILogger<CatSublineasController> logger, IConfiguration config,ICatSublineasServicio sublineasService)
        {
            _sublineasService = sublineasService;
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
                List<CatSublineas> sublineas = await _sublineasService.Lista();                
                resultado = CryptographyUtils.CrearResultado(sublineas);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las sublineas");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorSublinea")]
        public async Task<ResultadoAPI> ObtenerPorSublinea(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatSublineas>(r);
                var lineaResult = await _sublineasService.ObtenerPorSublinea(linea.Sublinea);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las sublineas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatSublineas")]
        public async Task<ResultadoAPI> IME_CatSublineas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var sublinea = CryptographyUtils.DeserializarPeticion<CatSublineas>(r);
                sublinea.Usuario = peticion.usuario;
                sublinea.Maquina = peticion.maquina;
                var lineaResult = await _sublineasService.IME_CatSublineas(sublinea);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la sublinea");
            }
            finally { }

            return resultado;
        }
    }
}
