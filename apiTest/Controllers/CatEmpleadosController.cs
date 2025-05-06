using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CatEmpleadosController : ControllerBase
    {
        private readonly ICatEmpleadosServicio _EmpleadoServicio ;
        private readonly ILogger<CatEmpleadosController > _logger;
        private readonly IConfiguration _config;

        public CatEmpleadosController(ILogger<CatEmpleadosController> logger, IConfiguration config, ICatEmpleadosServicio EmpleadoServicio)
        {
            _EmpleadoServicio = EmpleadoServicio ;
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
                List<CatEmpleados> lstEmpleados = await _EmpleadoServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(lstEmpleados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar los empleados");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerEmpleado")]
        public async Task<ResultadoAPI> ObtenerEmpleado(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var objEmpleado = CryptographyUtils.DeserializarPeticion<CatEmpleados>(r);
                var Result = await _EmpleadoServicio.ObtenerEmpleado (objEmpleado.Folio);
                resultado = CryptographyUtils.CrearResultado(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al obtener el empleado");
            }
            finally { }

            return resultado;
        }

        [HttpPost("IME_CAT_Empleados")]
        public async Task<ResultadoAPI> IME_CAT_Empleados(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var objEmpleado = CryptographyUtils.DeserializarPeticion<CatEmpleados >(r);
                objEmpleado.Usuario = peticion.usuario;
                objEmpleado.Maquina = peticion.maquina;
                var Result = await _EmpleadoServicio.IME_CAT_Empleados(objEmpleado);
                resultado = CryptographyUtils.CrearResultado(Result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar el empleado");
            }
            finally { }

            return resultado;
        }

    }
}
