using Voalaft.API.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Utilerias;

namespace Voalaft.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IUsuariosServicio _usuario;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _config;

        public AuthController(ILogger<AuthController> logger, IConfiguration config,IUsuariosServicio usuario)
        {
            _usuario = usuario;
            _logger = logger;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<ResultadoAPI> Login(PeticionAPI peticion)
        {
            //var peticion = HttpContext.Items["peticion"] as PeticionAPI;
            UsuarioLogin user = null;
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                user = CryptographyUtils.DeserializarPeticion<UsuarioLogin>(r);
                
                Console.WriteLine(user.usuario_id);
                Usuarios usuario = await _usuario.ObtenerPorUsuario(user.usuario_id);
                if(usuario==null)
                {
                    throw new Exception("Usuario no encontrado "+user.usuario_id);
                }
                else if(user.usuario_id.ToLower().CompareTo("admin")==0)
                    user.usuario_name = "Admin Portal";
                else
                {
                    user.usuario_name = ConvertUtils.ToString(usuario.Nombre) + " " + ConvertUtils.ToString(usuario.ApellidoPaterno);
                }
                List<MenuUsuario> listMenu = await _usuario.ObtenerMenuUsuario(usuario.Folio);
                if(listMenu != null)
                {
                    user.menuUsuarios = listMenu;
                }
                var token = GenerateToken(user);
                user.token = token;
                resultado = CryptographyUtils.CrearResultado(user);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message,ex);
                throw new Exception("Usuario y/o contraseña invalida");
            }
            finally { }
            
            return resultado; 
        }

        private string GenerateToken(UsuarioLogin user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.usuario_name),
                new Claim(ClaimTypes.NameIdentifier, user.usuario_id),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }

        [HttpPost("AccesoUsuario")]
        public async Task<ResultadoAPI> AccesoUsuario(PeticionAPI peticion)
        {
            //var peticion = HttpContext.Items["peticion"] as PeticionAPI;
            UsuarioLogin user = null;
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                user = CryptographyUtils.DeserializarPeticion<UsuarioLogin>(r);

                Console.WriteLine(user.usuario_id);
                Usuarios usuario = await _usuario.ObtenerPorUsuario(user.usuario_id);
                if (usuario == null)
                {
                    throw new Exception("Usuario no encontrado " + user.usuario_id);
                }
                else if (user.usuario_id.ToLower().CompareTo("admin") == 0)
                    user.usuario_name = "Admin Portal";
                else
                {
                    user.usuario_name = ConvertUtils.ToString(usuario.Nombre) + " " + ConvertUtils.ToString(usuario.ApellidoPaterno);
                }
                List<MenuUsuario> listMenu = await _usuario.ObtenerMenuUsuario(usuario.Folio);
                if (listMenu != null)
                {
                    user.menuUsuarios = listMenu;
                }
                var token = GenerateToken(user);
                user.token = token;
                resultado = CryptographyUtils.CrearResultado(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Usuario y/o contraseña invalida");
            }
            finally { }

            return resultado;
        }
    }
}
