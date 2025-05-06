using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatAlmacenesRepositorio
    {
        //Task<List<CatAlmacenes>> Lista(int nSucursal, int nPlaza, string CodigoPostal, string Descripcion);
        Task<CatAlmacenes> ObtenerAlmacen(int nAlmacen);
        Task<CatAlmacenes> IME_CatAlmacenes(CatAlmacenes objAlmacen);

    }
}
