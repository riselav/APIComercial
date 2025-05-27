using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatProveedoresServicio
    {
        Task<List<CatProveedores>> Lista();
        Task<CatProveedores> ObtenerProveedor(int nProveedor);
        Task<CatProveedores> IME_CatProveedores(CatProveedores catProveedor);
        Task<List<TableroProveedores>> ConsultaProveedores(ParametrosConsultaProveedores paramProveedores);

        Task<List<CatContactoProveedor>> ObtenerContactosProveedor(int nProveedor);
        Task<CatContactoProveedor> ObtenerContactoProveedorId(int nProveedor, int nContactoProveedor);
        Task<CatContactoProveedor> IME_CatContactoProveedores(CatContactoProveedor catContactoProveedor);
    }
}
