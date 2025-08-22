using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CatPlazasController : ControllerBase
    {
        private readonly ICatPlazasServicio _plazasService;
        private readonly ILogger<CatPlazasController> _logger;
        private readonly IConfiguration _config;

        public CatPlazasController(ILogger<CatPlazasController> logger, IConfiguration config, ICatPlazasServicio plazasService)
        {
            _plazasService = plazasService;
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
                List<CatPlaza> plazas = await _plazasService.Lista();
                resultado = CryptographyUtils.CrearResultado(plazas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las plazas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerPorPlaza")]
        public async Task<ResultadoAPI> ObtenerPorPlaza(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var plaza = CryptographyUtils.DeserializarPeticion<CatPlaza>(r);
                var plazaResult = await _plazasService.ObtenerPorPlaza(plaza.plaza);
                resultado = CryptographyUtils.CrearResultado(plazaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la plaza");
            }
            finally { }

            return resultado;
        }
    }
}
