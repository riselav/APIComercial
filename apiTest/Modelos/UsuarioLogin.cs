using Voalaft.Data.Entidades;

namespace Voalaft.API
{
    public class UsuarioLogin
    {
        public string usuario_id { get; set; }

        public int usuario_Empleado { get; set; }

        public string usuario_name { get; set; }                
        public string password { get; set; }
        public string token { get; set; }
        public List<MenuUsuario> menuUsuarios { get; set; }
}
}
