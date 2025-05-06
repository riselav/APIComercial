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
    public class CatArticulosController : ControllerBase
    {

        private readonly ICatArticulosServicio _articulosServicio;
        private readonly ILogger<CatArticulosController> _logger;
        private readonly IConfiguration _config;

        public CatArticulosController(ILogger<CatArticulosController> logger, IConfiguration config,ICatArticulosServicio articulosServicio)
        {
            _articulosServicio = articulosServicio;
            _logger = logger;
            _config = config;
        }

        
        [HttpPost("ListaSucursales")]
        public async Task<ResultadoAPI> ListaSucursales(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<SucursalesCombo> articulos = await _articulosServicio.ListaSucursales();
                resultado = CryptographyUtils.CrearResultado(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las sucursales");
            }
            finally { }

            return resultado;
        }
        [HttpPost("Lista")]
        public async Task<ResultadoAPI> Lista(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);                
                List<CatArticulos> articulos = await _articulosServicio.Lista();                
                resultado = CryptographyUtils.CrearResultado(articulos);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las articulos");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerPorArticulo")]
        public async Task<ResultadoAPI> ObtenerPorArticulo(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var articulo = CryptographyUtils.DeserializarPeticion<CatArticulos>(r);
                var articuloResult = await _articulosServicio.ObtenerPorArticulo(articulo.Articulo);
                resultado = CryptographyUtils.CrearResultado(articuloResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las articulos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatArticulos")]
        public async Task<ResultadoAPI> IME_CatArticulos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var articulo = CryptographyUtils.DeserializarPeticion<CatArticulos>(r);
                articulo.Usuario = peticion.usuario;
                articulo.Maquina = peticion.maquina;
                var articuloResult = await _articulosServicio.IME_CatArticulos(articulo);
                resultado = CryptographyUtils.CrearResultado(articuloResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la articulo");
            }
            finally { }

            return resultado;
        }
        
        [HttpPost("ConsultaTableroArticulos")]
        public async Task<ResultadoAPI> ConsultaTableroArticulos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {   
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaArticulo = CryptographyUtils.DeserializarPeticion<ParametrosConsultaArticulo>(r);
                List<ConsultaArticulo> articulos = await _articulosServicio.ConsultaTableroArticulos(parametrosConsultaArticulo);
                resultado = CryptographyUtils.CrearResultado(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las articulos");
            }
            finally { }

            return resultado;
        }
    }
}
