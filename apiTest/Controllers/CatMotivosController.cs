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
    public class CatMotivosController : ControllerBase
    {

        private readonly ICatMotivosServicio _motivosServicio;
        private readonly ILogger<CatMotivosController> _logger;
        private readonly IConfiguration _config;

        public CatMotivosController(ILogger<CatMotivosController> logger, IConfiguration config,ICatMotivosServicio motivosServicio)
        {
            _motivosServicio = motivosServicio;
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
                List<CatMotivos> motivos = await _motivosServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(motivos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las motivos");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorTipoMotivo")]
        public async Task<ResultadoAPI> ObtenerPorTipoMotivo(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var catalogo = CryptographyUtils.DeserializarPeticion<CatMotivos>(r);
                var catalogoResult = await _motivosServicio.ObtenerPorTipoMotivo(catalogo.nTipoMotivo);
                resultado = CryptographyUtils.CrearResultado(catalogoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las motivos");
            }
            finally { }

            return resultado;
        }
        
    }
}
