using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatCajaController : Controller
    {
        private readonly ICatCajaServicio _catCajaServicio;

        private readonly ILogger<CatCajaController> _logger;
        private readonly IConfiguration _config;

        public CatCajaController(ILogger<CatCajaController> logger, IConfiguration config,
                                    ICatCajaServicio catCajaServicio)
        {
            _catCajaServicio = catCajaServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaCatalogoCajas")]
        public async Task<ResultadoAPI> ListaCatalogoCajas(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatCaja> catCajas = await _catCajaServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catCajas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat cajas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaCatalogoCajas2")]
        public async Task<List<CatCaja>> ListaCatalogoCajas2()
        {
            List<CatCaja> catCajas = [];
            try
            {
                catCajas = await _catCajaServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat cajas");
            }
            finally { }

            return catCajas;
        }

        [HttpPost("ListaCajasPorSucursal")]
        public async Task<ResultadoAPI> ListaCajasPorSucursal(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaCajas = CryptographyUtils.DeserializarPeticion<ParametrosConsultaCajas>(r);
                List<CatCaja> catCajas = await _catCajaServicio.ObtenerPorSucursal(parametrosConsultaCajas.Sucursal);

                resultado = CryptographyUtils.CrearResultado(catCajas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat cajas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaCajasPorSucursal2")]
        public async Task<List<CatCaja>> ListaCajasPorSucursal2(long n_Sucursal)
        {
            List<CatCaja> catCajas = [];
            try
            {
                catCajas = await _catCajaServicio.ObtenerPorSucursal(n_Sucursal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat cajas");
            }
            finally { }

            return catCajas;
        }

        [HttpPost("ObtenerCajaPorId")]
        public async Task<ResultadoAPI> ObtenerCajaPorId(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaCajas = CryptographyUtils.DeserializarPeticion<ParametrosConsultaCajas>(r);
                CatCaja catCaja = await _catCajaServicio.ObtenerCajaPorId(parametrosConsultaCajas.Caja);

                resultado = CryptographyUtils.CrearResultado(catCaja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las empresas");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerCajaPorId2")]
        public async Task<CatCaja> ObtenerCajaPorId2(long n_Caja)
        {
            CatCaja resultado = null;
            try
            {

                resultado = await _catCajaServicio.ObtenerCajaPorId(n_Caja);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat Caja");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_Caja")]
        public async Task<ResultadoAPI> IME_Caja(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var caja = CryptographyUtils.DeserializarPeticion<CatCaja>(r);
                caja.Usuario = peticion.usuario;
                caja.Maquina = peticion.maquina;
                var cajaResult = await _catCajaServicio.IME_Caja(caja);
                resultado = CryptographyUtils.CrearResultado(cajaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar caja");
            }
            finally { }

            return resultado;
        }
    }
}