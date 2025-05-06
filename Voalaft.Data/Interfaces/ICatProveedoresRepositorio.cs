using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatProveedoresRepositorio
    {
        Task<List<CatProveedores >> Lista();
        Task<CatProveedores> ObtenerProveedor (int nProveedor);
        Task<CatProveedores> IME_CatProveedores(CatProveedores catProveedor);

    }
}
