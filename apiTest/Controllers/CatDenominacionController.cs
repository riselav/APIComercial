using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CatDenominacionController : Controller
    {
        private readonly ICatDenominacionServicio _catDenominacionServicio;

        private readonly ILogger<CatDenominacionController> _logger;
        private readonly IConfiguration _config;

        public CatDenominacionController(ILogger<CatDenominacionController> logger, IConfiguration config,
                                    ICatDenominacionServicio catDenominacionServicio)
        {
            _catDenominacionServicio = catDenominacionServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaDenominaciones")]
        public async Task<ResultadoAPI> ListaDenominaciones(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatDenominacion> catDenominaciones = await _catDenominacionServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catDenominaciones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat turnos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaDenominaciones2")]
        public async Task<List<CatDenominacion>> ListaDenominaciones2()
        {
            List<CatDenominacion> catDenominaciones = [];
            try
            {
                catDenominaciones = await _catDenominacionServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat denominaciones");
            }
            finally { }

            return catDenominaciones;
        }

        [HttpPost("ObtenerDenominacionPorId")]
        public async Task<CatDenominacion> ObtenerDenominacionPorId(long n_Denominacion)
        {
            CatDenominacion resultado = null;
            try
            {

                resultado = await _catDenominacionServicio.ObtenerPorId(n_Denominacion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat turno");
            }
            finally { }

            return resultado;
        }
    }
}
