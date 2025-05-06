using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IUsuariosServicio
    {
        Task<List<Usuarios>> Lista();
        Task<Usuarios> ObtenerPorUsuario(string usuario);
        Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario);
    }
}
