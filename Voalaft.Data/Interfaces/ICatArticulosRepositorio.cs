
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatArticulosRepositorio
    {
        Task<List<CatArticulos>> Lista();
        Task<CatArticulos> ObtenerPorArticulo(int nArticulo);
        Task<CatArticulos> IME_CatArticulos(CatArticulos catArticulo);
        Task<List<ConsultaArticulo>> ConsultaTableroArticulos(ParametrosConsultaArticulo parametrosConsultaArticulo);
        Task<List<SucursalesCombo>> ListaSucursales(); 
    }
}
