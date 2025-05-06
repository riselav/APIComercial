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
    public class CatUnidadesController : ControllerBase
    {

        private readonly ICatUnidadesServicio _unidadesServicio;
        private readonly ILogger<CatUnidadesController> _logger;
        private readonly IConfiguration _config;

        public CatUnidadesController(ILogger<CatUnidadesController> logger, IConfiguration config,ICatUnidadesServicio unidadesServicio)
        {
            _unidadesServicio = unidadesServicio;
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
                List<CatUnidades> unidades = await _unidadesServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(unidades);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las unidades");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorUnidad")]
        public async Task<ResultadoAPI> ObtenerPorUnidad(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var unidad = CryptographyUtils.DeserializarPeticion<CatUnidades>(r);
                var unidadResult = await _unidadesServicio.ObtenerPorUnidad(unidad.Unidad);
                resultado = CryptographyUtils.CrearResultado(unidadResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las unidades");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatUnidades")]
        public async Task<ResultadoAPI> IME_CatUnidades(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var unidad = CryptographyUtils.DeserializarPeticion<CatUnidades>(r);
                unidad.Usuario = peticion.usuario;
                unidad.Maquina = peticion.maquina;
                var unidadResult = await _unidadesServicio.IME_CatUnidades(unidad);
                resultado = CryptographyUtils.CrearResultado(unidadResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la unidad");
            }
            finally { }

            return resultado;
        }
    }
}
