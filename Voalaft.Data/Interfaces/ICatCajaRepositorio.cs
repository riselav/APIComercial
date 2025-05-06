using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatCajaRepositorio
    {
        Task<List<CatCaja>> Lista();
        Task<List<CatCaja>> ObtenerPorSucursal(long n_Sucursal);

        Task<CatCaja> ObtenerCajaPorId(long n_Caja);
    }
}