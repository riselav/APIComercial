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

    public class CatAlmacenesController : ControllerBase
    {

        private readonly ICatAlmacenesServicio  _AlmacenesService;
        private readonly ILogger<CatAlmacenesController > _logger;
        private readonly IConfiguration _config;

        public CatAlmacenesController(ILogger<CatAlmacenesController > logger, IConfiguration config, ICatAlmacenesServicio AlmacenesService)
        {
            _AlmacenesService = AlmacenesService;
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
                var objFiltros = CryptographyUtils.DeserializarPeticion<ConsultaTabAlmacenes>(r);
                List<CatAlmacenes> lstAlmacenes = []; // await _AlmacenesService.Lista (objFiltros.nSucursal, objFiltros.nPlaza, objFiltros.cDescripcion );
                resultado = CryptographyUtils.CrearResultado(lstAlmacenes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar los almacenes");
            }
            finally { }

            return resultado;
        }

    }
}
