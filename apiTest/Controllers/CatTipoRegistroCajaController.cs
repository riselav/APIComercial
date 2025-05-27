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

    public class CatTipoRegistroCajaController : Controller
    {
        private readonly ICatTipoRegistroCajaServicio _catTipoRegistroCajaServicio;

        private readonly ILogger<CatTipoRegistroCajaController> _logger;
        private readonly IConfiguration _config;

        public CatTipoRegistroCajaController(ILogger<CatTipoRegistroCajaController> logger, IConfiguration config,
                                    ICatTipoRegistroCajaServicio catTipoRegistroCajaServicio)
        {
            _catTipoRegistroCajaServicio = catTipoRegistroCajaServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaTiposRegistroCaja")]
        public async Task<ResultadoAPI> ListaTiposRegistroCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatTipoRegistroCaja> catTiposRegistrosCaja = await _catTipoRegistroCajaServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catTiposRegistrosCaja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat tipos de registro de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaTiposRegistroCaja2")]
        public async Task<List<CatTipoRegistroCaja>> ListaTiposRegistroCaja2()
        {
            List<CatTipoRegistroCaja> catTiposRegistroCaja = [];
            try
            {
                catTiposRegistroCaja = await _catTipoRegistroCajaServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat tipos de registro de caja");
            }
            finally { }

            return catTiposRegistroCaja;
        }

        [HttpPost("ObtenerTipoRegistroCajaPorId")]
        public async Task<CatTipoRegistroCaja> ObtenerTipoRegistroCajaPorId(int n_TipoRegistroCaja)
        {
            CatTipoRegistroCaja resultado = null;
            try
            {

                resultado = await _catTipoRegistroCajaServicio.ObtenerPorId(n_TipoRegistroCaja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat tipo de registro de caja");
            }
            finally { }

            return resultado;
        }

    }
}