using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatClientesServicio
    {
        Task<List<CatClientes>> Lista();
        Task<CatClientes> ObtenerPorId(long n_Cliente);
    }
}