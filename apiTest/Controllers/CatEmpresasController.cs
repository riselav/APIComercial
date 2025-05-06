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
    public class CatEmpresasController : ControllerBase
    {

        private readonly ICatEmpresasServicio _empresasService;
        private readonly ILogger<CatEmpresasController> _logger;
        private readonly IConfiguration _config;

        public CatEmpresasController(ILogger<CatEmpresasController> logger, IConfiguration config,ICatEmpresasServicio empresasService)
        {
            _empresasService = empresasService;
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
                List<CatEmpresas> empresas = await _empresasService.Lista();                
                resultado = CryptographyUtils.CrearResultado(empresas);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las empresas");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorEmpresa")]
        public async Task<ResultadoAPI> ObtenerPorEmpresa(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var linea = CryptographyUtils.DeserializarPeticion<CatEmpresas>(r);
                var lineaResult = await _empresasService.ObtenerPorEmpresa(linea.Empresa);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las empresas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatEmpresas")]
        public async Task<ResultadoAPI> IME_CatEmpresas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var empresa = CryptographyUtils.DeserializarPeticion<CatEmpresas>(r);
                empresa.Usuario = peticion.usuario;
                empresa.Maquina = peticion.maquina;
                var lineaResult = await _empresasService.IME_CatEmpresas(empresa);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la empresa");
            }
            finally { }

            return resultado;
        }
    }
}
