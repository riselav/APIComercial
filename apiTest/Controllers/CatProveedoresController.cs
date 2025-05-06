using Voalaft.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Utilerias;
using Microsoft.AspNetCore.Authorization;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatProveedoresController : ControllerBase    {

        private readonly ICatProveedoresServicio _ProveedorServicio;
        private readonly ILogger<CatProveedoresController> _logger;
        private readonly IConfiguration _config;

        public CatProveedoresController(ILogger<CatProveedoresController> logger, IConfiguration config, ICatProveedoresServicio  ProveedorServicio)
                    {
            _ProveedorServicio = ProveedorServicio;
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
                List<CatProveedores > lineas = await _ProveedorServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(lineas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las lineas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerProveedor")]
        public async Task<ResultadoAPI> ObtenerProveedor(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<CatProveedores >(r);
                var ProveedorResult = await _ProveedorServicio.ObtenerProveedor (Proveedor.Folio );
                resultado = CryptographyUtils.CrearResultado(ProveedorResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar  el proveedor");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatLineas")]
        public async Task<ResultadoAPI> IME_CatProveedores(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<CatProveedores>(r);
                Proveedor.cUsuario  = peticion.usuario;
                Proveedor.cMaquina  = peticion.maquina;
                var proveedorResult = await _ProveedorServicio.IME_CatProveedores (Proveedor);
                resultado = CryptographyUtils.CrearResultado(proveedorResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar el proveedor");
            }
            finally { }

            return resultado;
        }

    }
}
