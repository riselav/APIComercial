using Voalaft.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using Voalaft.API.Servicios.Implementacion;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatDatosDireccionController : ControllerBase
    {

        private readonly ICatDatosDireccionServicio _datosDireccionServicio;
        private readonly ILogger<CatDatosDireccionController> _logger;
        private readonly IConfiguration _config;

        public CatDatosDireccionController(ILogger<CatDatosDireccionController> logger, IConfiguration config, ICatDatosDireccionServicio datosDireccionServicio)
        {
            _datosDireccionServicio = datosDireccionServicio;
            _logger = logger;
            _config = config;
        }

        [HttpPost("ListaCodigosPostales")]
        public async Task<ResultadoAPI> ListaCodigosPostales(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);                
                List<CatCodigosPostales> catalogos = await _datosDireccionServicio.ListaCodigosPostales();                
                resultado = CryptographyUtils.CrearResultado(catalogos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar los codigos postales");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerColoniasPorCP")]
        public async Task<ResultadoAPI> ObtenerColoniasPorCP(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var catalogo = CryptographyUtils.DeserializarPeticion<CatColonias>(r);
                var catalogoResult = await _datosDireccionServicio.ObtenerColoniasPorCP(catalogo.CodigoPostal);
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

       [HttpPost("DireccionPorCodigoPostal")]
        public async Task<ResultadoAPI> DireccionPorCodigoPostal(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                JObject json = JObject.Parse(r);

                string? cCodigoPostal = json["cCodigoPostal"].Value<String>();

                //int nCaja = json["nCaja"].Value<int>();

                var datoDireccionResult = await _datosDireccionServicio.DireccionPorCodigoPostal(cCodigoPostal);
                resultado = CryptographyUtils.CrearResultado(datoDireccionResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar los datos de direccion por codigo postal");
            }
            finally { }

            return resultado;
        }
    }
}
