using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;

namespace Voalaft.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]

    public class CatClientesController : Controller
    {

        private readonly ICatClientesServicio _catClientesServicio;

        private readonly ILogger<CatClientesController> _logger;
        private readonly IConfiguration _config;

        public CatClientesController(ILogger<CatClientesController> logger, IConfiguration config,
                                    ICatClientesServicio catClientesServicio)
        {
            _catClientesServicio = catClientesServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ListaClientes")]
        public async Task<ResultadoAPI> ListaClientes(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                List<CatClientes> catClientes = await _catClientesServicio.Lista();
                resultado = CryptographyUtils.CrearResultado(catClientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat clientes");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ListaClientes2")]
        public async Task<List<CatClientes>> ListaClientes2()
        {
            List<CatClientes> catClientes = [];
            try
            {
                catClientes = await _catClientesServicio.Lista();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat cajas");
            }
            finally { }

            return catClientes;
        }

        [HttpPost("ObtenerClientePorId")]
        public async Task<CatClientes> ObtenerClientePorId(long n_Cliente)
        {
            CatClientes resultado = null;
            try
            {

                resultado = await _catClientesServicio.ObtenerPorId(n_Cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar el registro de cat cliente");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ConsultaClientes")]
        public async Task<ResultadoAPI> ConsultaClientes(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var Cliente = CryptographyUtils.DeserializarPeticion<ParametrosConsultaClientes>(r);
                List<Cliente> cli = await _catClientesServicio.ConsultaClientes(Cliente);
                resultado = CryptographyUtils.CrearResultado(cli);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar los clientes de tablero");
            }
            finally { }

            return resultado;
        }
    }
}
