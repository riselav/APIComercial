using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatProveedoresServicio
    {
        Task<List<CatProveedores>> Lista();
        Task<CatProveedores> ObtenerProveedor(int nProveedor);
        Task<CatProveedores> IME_CatProveedores(CatProveedores catProveedor);
    }
}
