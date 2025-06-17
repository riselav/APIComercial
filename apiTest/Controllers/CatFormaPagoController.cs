
﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatFormaPagoController : Controller
    {
        private readonly ICatFormaPagoServicio _catFormaPagoServicio;

        private readonly ILogger<CatFormaPagoController> _logger;
        private readonly IConfiguration _config;

        public CatFormaPagoController(ILogger<CatFormaPagoController> logger, IConfiguration config,
                                    ICatFormaPagoServicio catFormaPagoServicio)
        {
            _catFormaPagoServicio = catFormaPagoServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaFormasPago")]
        public async Task<ResultadoAPI> ListaFormasPago(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatFormaPago> catFormasPago = await _catFormaPagoServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catFormasPago);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat formas de pago");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaFormasPago2")]
        public async Task<List<CatFormaPago>> ListaFormasPago2()
        {
            List<CatFormaPago> catFormasPago = [];
            try
            {
                catFormasPago = await _catFormaPagoServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat formas de pago");
            }
            finally { }

            return catFormasPago;
        }

        [HttpPost("ListaFormasPagoPorTipoEgreso")]
        public async Task<List<CatFormaPago>> ListaFormasPagoPorTipoEgreso(int n_TipoEgreso)
        {
            List<CatFormaPago> catFormasPago = [];
            try
            {
                catFormasPago = await _catFormaPagoServicio.ObtenerPorTipoEgreso(n_TipoEgreso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat formas de pago por tipo de egreso");
            }
            finally { }

            return catFormasPago;
        }

        [HttpPost("ObtenerFormaPagoPorId")]
        public async Task<CatFormaPago> ObtenerFormaPagoPorId(long n_FormaPago)
        {
            CatFormaPago resultado = null;
            try
            {

                resultado = await _catFormaPagoServicio.ObtenerPorId(n_FormaPago);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat cliente");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerImportesFormaPagoApertura")]
        public async Task<ResultadoAPI> ObtenerImportesFormaPagoApertura(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                JObject json = JObject.Parse(r);

                int nSucursal = json["nSucursal"].Value<int>();
                int nCaja = json["nCaja"].Value<int>();

                var articuloResult = await _catFormaPagoServicio.ObtenerImportesFormaPagoApertura(nSucursal, nCaja);
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

    }
}