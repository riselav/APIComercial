using Voalaft.API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {      
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(ILogger<UsuariosController> logger)
        {
            _logger = logger;
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
    }
}
