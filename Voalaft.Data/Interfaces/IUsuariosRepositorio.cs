
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface IUsuariosRepositorio
    {
        Task<List<Usuarios>> Lista();
        Task<Usuarios> ObtenerPorUsuario(string usuario);
        Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario);

        Task<Usuarios> AccesoUsuario(Usuarios usuario);
    }
}