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
    public class CatCatalogosController : ControllerBase
    {

        private readonly ICatCatalogosServicio _catalogosServicio;
        private readonly ILogger<CatCatalogosController> _logger;
        private readonly IConfiguration _config;

        public CatCatalogosController(ILogger<CatCatalogosController> logger, IConfiguration config,ICatCatalogosServicio catalogosServicio)
        {
            _catalogosServicio = catalogosServicio;
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
                List<CatCatalogos> catalogos = await _catalogosServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(catalogos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las catalogos");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorNombre")]
        public async Task<ResultadoAPI> ObtenerPorNombre(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var catalogo = CryptographyUtils.DeserializarPeticion<CatCatalogos>(r);
                var catalogoResult = await _catalogosServicio.ObtenerPorNombre(catalogo.Nombre);
                resultado = CryptographyUtils.CrearResultado(catalogoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catalogos");
            }
            finally { }

            return resultado;
        }
        
    }
}
