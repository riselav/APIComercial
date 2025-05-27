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
using Voalaft.Data.Entidades.Tableros;
using Voalaft.Data.Entidades.ClasesParametros;

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

        
        [HttpPost("ConsultaProveedores")]
        public async Task<ResultadoAPI> ConsultaProveedores(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<ParametrosConsultaProveedores>(r);
                List<TableroProveedores> prov = await _ProveedorServicio.ConsultaProveedores(Proveedor);
                resultado = CryptographyUtils.CrearResultado(prov);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las proveedores");
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

        [HttpPost("IME_CatProveedores")]
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
        //[HttpPost("ObtenerRFC")]
        //public async Task<ResultadoAPI> ObtenerRFC(PeticionAPI peticion)
        //{
        //    ResultadoAPI resultado = null;
        //    try
        //    {
        //        var r = CryptographyUtils.Desencriptar(peticion.contenido);
        //        var Proveedor = CryptographyUtils.DeserializarPeticion<CatRFC>(r);
        //        var ProveedorResult = await _ProveedorServicio.ObtenerProveedor(Proveedor.nIDRFC);
        //        resultado = CryptographyUtils.CrearResultado(ProveedorResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //        throw new Exception("Error al consultar el RFC");
        //    }
        //    finally { }

        //    return resultado;
        //}

        [HttpPost("IME_CatContactoProveedores")]
        public async Task<ResultadoAPI> IME_CatContactoProveedores(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<CatContactoProveedor>(r);
                Proveedor.Usuario = peticion.usuario;
                Proveedor.Maquina = peticion.maquina;
                var proveedorResult = await _ProveedorServicio.IME_CatContactoProveedores(Proveedor);
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

        [HttpPost("ObtenerContactosProveedor")]
        public async Task<ResultadoAPI> ObtenerContactosProveedor(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<CatContactoProveedor>(r);
                Proveedor.Usuario = peticion.usuario;
                Proveedor.Maquina = peticion.maquina;
                var proveedorResult = await _ProveedorServicio.ObtenerContactosProveedor(Proveedor.nProveedor);
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
        [HttpPost("ObtenerContactoProveedorId")]
        public async Task<ResultadoAPI> ObtenerContactoProveedorId(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Proveedor = CryptographyUtils.DeserializarPeticion<CatContactoProveedor>(r);
                Proveedor.Usuario = peticion.usuario;
                Proveedor.Maquina = peticion.maquina;
                var proveedorResult = await _ProveedorServicio.ObtenerContactoProveedorId(Proveedor.nProveedor,Proveedor.nContactoProveedor);
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
