
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Menu;

namespace Voalaft.Data.Interfaces
{
    public interface IUsuariosRepositorio
    {
        Task<List<Usuarios>> Lista();
        Task<Usuarios> ObtenerPorUsuario(string usuario);
        Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario);
        Task<List<MenuDataRow>> get_menu_web_usuario(int nUsuario);

        Task<Usuarios> AccesoUsuario(Usuarios usuario);

        Task<Usuarios> AccesoEmpleado(Usuarios Empleado);
    }
}