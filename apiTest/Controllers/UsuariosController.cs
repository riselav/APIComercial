using Voalaft.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voalaft.Data.Entidades;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Servicios.Implementacion;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosServicio _usuariosServicio;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger, IUsuariosServicio usuariosServicio)
        {
            _logger = logger;
            _usuariosServicio = usuariosServicio;
        }

        [HttpPost(Name = "GetUsuario")]
        public UsuarioLogin Get(PeticionAPI peticion)
        {
            Console.WriteLine(peticion.contenido);
            UsuarioLogin user = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                user = CryptographyUtils.DeserializarPeticion<UsuarioLogin>(r);
                Console.WriteLine(user.usuario_id);
            }
            finally { }

            return user; ;
        }

        [HttpPost("AccesoUsuario")]
        public async Task<ResultadoAPI> AccesoUsuario(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var usuario = CryptographyUtils.DeserializarPeticion<Usuarios>(r);
                Usuarios usuarioValido = await _usuariosServicio.AccesoUsuario(usuario);
                resultado = CryptographyUtils.CrearResultado(usuarioValido);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar la lista de cat turnos");
            }
            finally { }

            return resultado;
        }
    }
}
