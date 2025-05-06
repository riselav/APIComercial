using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatSucursalesRepositorio
    {
        Task<List<CatSucursales >> Lista();
        Task<CatSucursales> ObtenerSucursal(int nSucursal);
        Task<CatSucursales> IME_CatSucursales(CatSucursales catSucursal);
    }
}
