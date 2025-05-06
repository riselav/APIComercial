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
    public class CatMarcasController : ControllerBase
    {

        private readonly ICatMarcasServicio _marcasService;
        private readonly ILogger<CatMarcasController> _logger;
        private readonly IConfiguration _config;

        public CatMarcasController(ILogger<CatMarcasController> logger, IConfiguration config,ICatMarcasServicio marcasService)
        {
            _marcasService = marcasService;
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
                List<CatMarcas> marcas = await _marcasService.Lista();                
                resultado = CryptographyUtils.CrearResultado(marcas);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las marcas");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorMarca")]
        public async Task<ResultadoAPI> ObtenerPorMarca(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatMarcas>(r);
                var lineaResult = await _marcasService.ObtenerPorMarca(linea.Marca);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las marcas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatMarcas")]
        public async Task<ResultadoAPI> IME_CatMarcas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var marca = CryptographyUtils.DeserializarPeticion<CatMarcas>(r);
                marca.Usuario = peticion.usuario;
                marca.Maquina = peticion.maquina;
                var lineaResult = await _marcasService.IME_CatMarcas(marca);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la marca");
            }
            finally { }

            return resultado;
        }
    }
}
