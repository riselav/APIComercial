using Voalaft.API.Utils;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Microsoft.AspNetCore.Authorization;
using Voalaft.API.Servicios.Implementacion;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CatalagosSATController : ControllerBase
    {

        private readonly ICatalagosSATServicio _catalagosSATServicio;
        private readonly ISatImpuestoServicio _satImpuestoServicio;
        private readonly ISatTipoFactorServicio _satTipoFactorServicio;

        private readonly ISatRegimenFiscalServicio _satRegimenFiscalServicio;
        private readonly ISatCatUsoCFDIServicio _satCatUsoCFDIServicio;
        private readonly ICatRFCServicio _catRFCServicio;

        private readonly ICatCorreoContactoRFCServicio _catCorreoContactoRFCServicio;

        private readonly ILogger<CatalagosSATController> _logger;
        private readonly IConfiguration _config;

        public CatalagosSATController(ILogger<CatalagosSATController> logger, IConfiguration config, 
                                    ICatalagosSATServicio catalagosSATServicio, 
                                    ISatImpuestoServicio satImpuestoServicio, ISatTipoFactorServicio satTipoFactorServicio, 
                                    ISatRegimenFiscalServicio satRegimenFiscalServicio,ISatCatUsoCFDIServicio satCatUsoCFDIServicio,
                                    ICatRFCServicio catRFCServicio, ICatCorreoContactoRFCServicio catCorreoContactoRFCServicio)
        {
            _catalagosSATServicio = catalagosSATServicio;
            _satImpuestoServicio = satImpuestoServicio;
            _satTipoFactorServicio = satTipoFactorServicio;

            _logger = logger;
            _config = config;
         
            _satRegimenFiscalServicio = satRegimenFiscalServicio;
            _satCatUsoCFDIServicio = satCatUsoCFDIServicio;
            _catRFCServicio = catRFCServicio;
            _catCorreoContactoRFCServicio = catCorreoContactoRFCServicio;
        }

        [HttpPost("ListaCatUnidades")]
        public async Task<ResultadoAPI> ListaCatUnidades(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);                
                List<CatUnidadesSat> catUnidadesSat = await _catalagosSATServicio.ListaCatUnidades();                
                resultado = CryptographyUtils.CrearResultado(catUnidadesSat);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Error al consultar las catUnidadesSat");
            }
            finally { }
            
            return resultado; 
        }

        [HttpPost("ObtenerUnidadesPorClave")]
        public async Task<ResultadoAPI> ObtenerUnidadesPorClave(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var unidadesSat = CryptographyUtils.DeserializarPeticion<CatUnidadesSat>(r);
                var catalagoResult = await _catalagosSATServicio.ObtenerUnidadesPorClave(unidadesSat.Clave);
                resultado = CryptographyUtils.CrearResultado(catalagoResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catUnidadesSat");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaSatImpuesto")]
        public async Task<ResultadoAPI> ListaSatImpuesto(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<SatImpuesto> satImpuestos = await _satImpuestoServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(satImpuestos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las satImpuestos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaSatTipoFactor")]
        public async Task<ResultadoAPI> ListaSatTipoFactor(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<SatTipoFactor> satTipoFactor = await _satTipoFactorServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(satTipoFactor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las satTipoFactor");
            }
            finally { }

            return resultado;
        }

        //***********************************
        [HttpPost("ListaRegimenFiscal")]
        public async Task<ResultadoAPI> ListaRegimenFiscal(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<SatRegimenFiscal> catSatRegimenFiscal = await _satRegimenFiscalServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catSatRegimenFiscal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catSatRegimenFiscal");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaRegimenFiscal2")]
        public async Task<List<SatRegimenFiscal>> ListaRegimenFiscal2()
        {
            List<SatRegimenFiscal> catSatRegimenFiscal = null;
            try
            {
                catSatRegimenFiscal = await _satRegimenFiscalServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catSatRegimenFiscal");
            }
            finally { }

            return catSatRegimenFiscal;
        }

        [HttpPost("ObtenerPorId")]
        public async Task<ResultadoAPI> ObtenerPorId(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var RegimenFiscal = CryptographyUtils.DeserializarPeticion<SatRegimenFiscal>(r);
                var lineaResult = await _satRegimenFiscalServicio.ObtenerPorId((long) RegimenFiscal.nIdRegimenFiscal);
                resultado = CryptographyUtils.CrearResultado(lineaResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catUnidadesSat");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerPorId2")]
        public async Task<SatRegimenFiscal> ObtenerPorId2(long nRegimenFiscal)
        {
            SatRegimenFiscal resultado = null;
            try
            {

                 resultado = await _satRegimenFiscalServicio.ObtenerPorId(nRegimenFiscal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catUnidadesSat");
            }
            finally { }

            return resultado;
        }

        //***********************************
        [HttpPost("ListaCatUsoCFDI")]
        public async Task<ResultadoAPI> ListaCatUsoCFDI(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<SatCatUsoCFDI> satCatUsoCFDI = await _satCatUsoCFDIServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(satCatUsoCFDI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de uso de cfdi");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaCatUsoCFDI2")]
        public async Task<List<SatCatUsoCFDI>> ListaCatUsoCFDI2()
        {
            List<SatCatUsoCFDI> satCatUsoCFDI = null;
            try
            {
                satCatUsoCFDI = await _satCatUsoCFDIServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de uso de cfdi");
            }
            finally { }

            return satCatUsoCFDI;
        }

        [HttpPost("ObtenerUsoCFDIPorClave")]
        public async Task<ResultadoAPI> ObtenerUsoCFDIPorClave(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var satCatUsoCFDI = CryptographyUtils.DeserializarPeticion<SatCatUsoCFDI>(r);
                var satCatUsoCFDIResult = await _satCatUsoCFDIServicio.ObtenerPorClave(satCatUsoCFDI.UsoCFDI);
                resultado = CryptographyUtils.CrearResultado(satCatUsoCFDIResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el Uso CFDI");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerUsoCFDIPorClave2")]
        public async Task<SatCatUsoCFDI> ObtenerUsoCFDIPorClave2(string cUsoCFDI)
        {
            SatCatUsoCFDI resultado = null;
            try
            {

                resultado = await _satCatUsoCFDIServicio.ObtenerPorClave(cUsoCFDI);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el uso CFDI");
            }
            finally { }

            return resultado;
        }

        //***********************************
        [HttpPost("ListaCatRFC")]
        public async Task<ResultadoAPI> ListaCatRFC(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatRFC> catRFC= await _catRFCServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catRFC);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat rfc");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaCatRFC2")]
        public async Task<List<CatRFC>> ListaCatRFC2()
        {
            List<CatRFC> catRFC = null;
            try
            {
                catRFC = await _catRFCServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat rfc");
            }
            finally { }

            return catRFC;
        }

        [HttpPost("ObtenerPorRFC")]
        public async Task<ResultadoAPI> ObtenerPorRFC(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var catRFC = CryptographyUtils.DeserializarPeticion<CatRFC>(r);
                var catRFCResult = await _catRFCServicio.ObtenerPorRFC(catRFC.cRFC);
                resultado = CryptographyUtils.CrearResultado(catRFCResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el Cat RFC");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerPorRFC2")]
        public async Task<CatRFC> ObtenerPorRFC2(string cRFC)
        {
            CatRFC resultado = null;
            try
            {

                resultado = await _catRFCServicio.ObtenerPorRFC(cRFC);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el cat RFC");
            }
            finally { }

            return resultado;
        }

        //***********************************
        [HttpPost("ListaContactosCorreoRFC")]
        public async Task<ResultadoAPI> ListaContactosCorreoRFC(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosConsultaCorreosRFC = CryptographyUtils.DeserializarPeticion<ParametrosConsultaCorreosRFC>(r);
                List<CatCorreoContactoRFC> catCorreoContactoRFC = await _catCorreoContactoRFCServicio.ObtenerPorId(parametrosConsultaCorreosRFC.IDRFC);

                resultado = CryptographyUtils.CrearResultado(catCorreoContactoRFC);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las los correos del RFC");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaContactosCorreoRFC2")]
        public async Task<List<CatCorreoContactoRFC>> ListaContactosCorreoRFC(long n_IDRFC)
        {
            List<CatCorreoContactoRFC> catCorreoContactoRFC = null;
            try
            {
                catCorreoContactoRFC = await _catCorreoContactoRFCServicio.ObtenerPorId(n_IDRFC);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar las catSatRegimenFiscal");
            }
            finally { }

            return catCorreoContactoRFC;
        }
    }
}