using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatArticuloVentaController : Controller
    {
        private readonly IArticuloVentaServicio _articuloVentaServicio;
        private readonly ILogger<CatArticuloVentaController> _logger;
        private readonly IConfiguration? _config;

        public CatArticuloVentaController(ILogger<CatArticuloVentaController> logger, IConfiguration config, IArticuloVentaServicio articulosServicio)
        {
            _articuloVentaServicio = articulosServicio;
            _logger = logger;
            _config = config;
        }

        [HttpPost("ObtenArticulosVenta")]
        public async Task<ResultadoAPI> ObtenArticulosVenta(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var ParametrosObtenArticulosVenta = CryptographyUtils.DeserializarPeticion<ParametrosObtenArticulosVenta>(r);
                List<CatArticuloVenta> articulos = await _articuloVentaServicio.ObtenArticulosVenta(ParametrosObtenArticulosVenta);
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

        [HttpPost("ObtenArticulosVenta2")]
        public async Task<List<CatArticuloVenta>> ObtenArticulosVenta2(ParametrosObtenArticulosVenta ParametrosObtenArticulosVenta)
        {
            List<CatArticuloVenta> articulos = null;
            try
            {
                //var r = CryptographyUtils.Desencriptar(peticion.contenido);
                //var ParametrosObtenArticulosVenta = CryptographyUtils.DeserializarPeticion<ParametrosObtenArticulosVenta>(r);
                articulos = await _articuloVentaServicio.ObtenArticulosVenta(ParametrosObtenArticulosVenta);
                                
                //resultado = CryptographyUtils.CrearResultado(articulos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las articulos");
            }
            finally { }

            return articulos;
        }

        //[HttpPost("ObtenerPorArticulo")]
        //public async Task<ResultadoAPI> ObtenArticulosVenta(PeticionAPI peticion)
        //{
        //    ResultadoAPI resultado = null;
        //    try
        //    {
        //        var r = CryptographyUtils.Desencriptar(peticion.contenido);
        //        var articulo = CryptographyUtils.DeserializarPeticion<CatArticulos>(r);
        //        var articuloResult = await _articulosServicio.ObtenerPorArticulo(articulo.Articulo);
        //        resultado = CryptographyUtils.CrearResultado(articuloResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //        throw new Exception("Error al consultar las articulos");
        //    }
        //    finally { }

        //    return resultado;
        //}

    }
}
