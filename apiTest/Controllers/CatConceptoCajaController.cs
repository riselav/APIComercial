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
    public class CatConceptoCajaController : Controller
    {
        private readonly ICatConceptoCajaServicio _catConceptoCajaServicio;

        private readonly ILogger<CatConceptoCajaController> _logger;
        private readonly IConfiguration _config;

        public CatConceptoCajaController(ILogger<CatConceptoCajaController> logger, IConfiguration config,
                                    ICatConceptoCajaServicio catConceptoCajaServicio)
        {
            _catConceptoCajaServicio = catConceptoCajaServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaConceptosCaja")]
        public async Task<ResultadoAPI> ListaConceptosCaja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatConceptoCaja> catConceptosCaja = await _catConceptoCajaServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catConceptosCaja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat conceptos de caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaConceptosCaja2")]
        public async Task<List<CatConceptoCaja>> ListaConceptosCaja2()
        {
            List<CatConceptoCaja> catConceptosCaja = [];
            try
            {
                catConceptosCaja = await _catConceptoCajaServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat conceptos de caja");
            }
            finally { }

            return catConceptosCaja;
        }

        [HttpPost("ListaConceptosCajaPorEfecto")]
        public async Task<List<CatConceptoCaja>> ListaConceptosCajaPorEfecto(int n_Efecto)
        {
            List<CatConceptoCaja> catConceptosCaja = [];
            try
            {
                catConceptosCaja = await _catConceptoCajaServicio.ObtenerPorEfecto(n_Efecto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat formas de pago por tipo de egreso");
            }
            finally { }

            return catConceptosCaja;
        }

        [HttpPost("ObtenerConceptoCajaPorId")]
        public async Task<CatConceptoCaja> ObtenerConceptoCajaPorId(long n_ConceptoCaja)
        {
            CatConceptoCaja resultado = null;
            try
            {

                resultado = await _catConceptoCajaServicio.ObtenerPorId(n_ConceptoCaja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat cliente");
            }
            finally { }

            return resultado;
        }
    }
}
