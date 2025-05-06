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
    public class CatGrupoEmpresarialController : ControllerBase
    {

        private readonly ICatGrupoEmpresarialServicio _grupoEmpresarialService;
        private readonly ILogger<CatGrupoEmpresarialController> _logger;
        private readonly IConfiguration _config;

        public CatGrupoEmpresarialController(ILogger<CatGrupoEmpresarialController> logger, IConfiguration config,ICatGrupoEmpresarialServicio grupoEmpresarialService)
        {
            _grupoEmpresarialService = grupoEmpresarialService;
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
                List<CatGrupoEmpresarial> grupoEmpresarial = await _grupoEmpresarialService.Lista();                
                resultado = CryptographyUtils.CrearResultado(grupoEmpresarial);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las grupoEmpresarial");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorGrupoEmpresaria")]
        public async Task<ResultadoAPI> ObtenerPorGrupoEmpresaria(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatGrupoEmpresarial>(r);
                var lineaResult = await _grupoEmpresarialService.ObtenerPorGrupoEmpresarial(linea.GrupoEmpresarial);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las grupoEmpresarial");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatGrupoEmpresarial")]
        public async Task<ResultadoAPI> IME_CatGrupoEmpresarial(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var grupoEmpresarial = CryptographyUtils.DeserializarPeticion<CatGrupoEmpresarial>(r);
                grupoEmpresarial.Usuario = peticion.usuario;
                grupoEmpresarial.Maquina = peticion.maquina;
                var lineaResult = await _grupoEmpresarialService.IME_CatGrupoEmpresarial(grupoEmpresarial);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la grupoEmpresarial");
            }
            finally { }

            return resultado;
        }
    }
}
