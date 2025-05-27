using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CatTurnoController : Controller
    {
        private readonly ICatTurnoServicio _catTurnoServicio;

        private readonly ILogger<CatTurnoController> _logger;
        private readonly IConfiguration _config;

        public CatTurnoController(ILogger<CatTurnoController> logger, IConfiguration config,
                                    ICatTurnoServicio catTurnoServicio)
        {
            _catTurnoServicio = catTurnoServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaTurnos")]
        public async Task<ResultadoAPI> ListaTurnos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatTurno> catTurnos = await _catTurnoServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catTurnos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat turnos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaTurnos2")]
        public async Task<List<CatTurno>> ListaTurnos2()
        {
            List<CatTurno> catTurnos = [];
            try
            {
                catTurnos = await _catTurnoServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat turnos");
            }
            finally { }

            return catTurnos;
        }

        [HttpPost("ObtenerTurnoPorId")]
        public async Task<CatTurno> ObtenerTurnoPorId(long n_Turno)
        {
            CatTurno resultado = null;
            try
            {

                resultado = await _catTurnoServicio.ObtenerPorId(n_Turno);
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
