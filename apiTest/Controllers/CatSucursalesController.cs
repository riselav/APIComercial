using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatSucursalesController : ControllerBase
    {
        private readonly ICatSucursalesRepositorio  _SucursalesService;
        private readonly ILogger<CatSucursalesController> _logger;
        private readonly IConfiguration _config;

        public CatSucursalesController(ILogger<CatSucursalesController> logger, IConfiguration config, ICatSucursalesRepositorio SucursalesService)
        {
            _SucursalesService = SucursalesService;
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
                List<CatSucursales > lstSucursales = await _SucursalesService.Lista();
                resultado = CryptographyUtils.CrearResultado(lstSucursales);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las Sucursales");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerSucursal")]
        public async Task<ResultadoAPI> ObtenerSucursal(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Sucursal = CryptographyUtils.DeserializarPeticion<CatSucursales >(r);
                var SucursalResult = await _SucursalesService.ObtenerSucursal (Sucursal.nSucursal);
                resultado = CryptographyUtils.CrearResultado(SucursalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la Sucursal");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CatSucursales")]
        public async Task<ResultadoAPI> IME_CatSucursales(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Sucursal = CryptographyUtils.DeserializarPeticion<CatSucursales>(r);
                Sucursal.Usuario = peticion.usuario;
                Sucursal.Maquina = peticion.maquina;
                var SucursalResult = await _SucursalesService.IME_CatSucursales (Sucursal);
                resultado = CryptographyUtils.CrearResultado(SucursalResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar la Sucursal");
            }
            finally { }

            return resultado;
        }

    }
}
