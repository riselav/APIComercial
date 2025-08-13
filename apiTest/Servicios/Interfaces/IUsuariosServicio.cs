using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Menu;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IUsuariosServicio
    {
        Task<List<Usuarios>> Lista();
        Task<Usuarios> ObtenerPorUsuario(string usuario);
        Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario);
        //Task<MenuNavigationRoute> get_menu_web_usuario(int nUsuario);
        Task<Dictionary<string, object>> get_menu_web_usuario(int nUsuario);
        
        Task<Usuarios> AccesoUsuario(Usuarios usuario);

        Task<Usuarios> AccesoEmpleado(Usuarios Empleado);
    }
}