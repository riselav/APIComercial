using Microsoft.AspNetCore.Mvc;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Voalaft.API.Controllers
{
    [Authorize]
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
        public async Task<ResultadoAPI> ObtenerClientePorId(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                JObject json = JObject.Parse(r);

                long n_Cliente = json["nCliente"].Value<long>();
                var ClienteResult = await _catClientesServicio.ObtenerPorId(n_Cliente);
                resultado = CryptographyUtils.CrearResultado(ClienteResult);
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

        [HttpPost("IME_Cliente")]
        public async Task<ResultadoAPI> IME_Cliente(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var cliente = CryptographyUtils.DeserializarPeticion<CatClientes>(r);
                cliente.Usuario = peticion.usuario;
                cliente.Maquina = peticion.maquina;
                var clienteResult = await _catClientesServicio.IME_Cliente(cliente);
                resultado = CryptographyUtils.CrearResultado(clienteResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al guardar cliente");
            }
            finally { }

            return resultado;
        }

        [HttpPost("EliminarContactoCliente")]
        public async Task<ResultadoAPI> EliminarContactoCliente(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var contacto = CryptographyUtils.DeserializarPeticion<ContactoCliente>(r);
                contacto.usuario = peticion.usuario;
                contacto.maquina = peticion.maquina;
                var clienteResult = await _catClientesServicio.EliminarContactoCliente(contacto);
                resultado = CryptographyUtils.CrearResultado(clienteResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al eliminar contacto de cliente");
            }
            finally { }

            return resultado;
        }
    }
}
