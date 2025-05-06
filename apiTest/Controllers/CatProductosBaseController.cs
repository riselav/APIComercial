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
    public class CatProductosBaseController : ControllerBase
    {

        private readonly ICatProductosBaseServicio _productosBaseServicio;
        private readonly ILogger<CatProductosBaseController> _logger;
        private readonly IConfiguration _config;

        public CatProductosBaseController(ILogger<CatProductosBaseController> logger, IConfiguration config,ICatProductosBaseServicio productosBaseServicio)
        {
            _productosBaseServicio = productosBaseServicio;
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
                List<CatProductosBase> productosBase = await _productosBaseServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(productosBase);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las productosBase");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorProductoBase")]
        public async Task<ResultadoAPI> ObtenerPorProductoBase(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var productoBase = CryptographyUtils.DeserializarPeticion<CatProductosBase>(r);
                var productoBaseResult = await _productosBaseServicio.ObtenerPorProductoBase(productoBase.ProductoBase);
                resultado = CryptographyUtils.CrearResultado(productoBaseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las productosBase");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatProductosBase")]
        public async Task<ResultadoAPI> IME_CatProductosBase(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var productoBase = CryptographyUtils.DeserializarPeticion<CatProductosBase>(r);
                productoBase.Usuario = peticion.usuario;
                productoBase.Maquina = peticion.maquina;
                var productoBaseResult = await _productosBaseServicio.IME_CatProductosBase(productoBase);
                resultado = CryptographyUtils.CrearResultado(productoBaseResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la productoBase");
            }
            finally { }

            return resultado;
        }
    }
}
