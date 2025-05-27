using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatFormaPagoRepositorio
    {
        Task<List<CatFormaPago>> Lista();
        Task<CatFormaPago> ObtenerPorId(long n_FormaPago);

        Task<List<CatFormaPago>> ObtenerPorTipoEgreso(int n_TipoEgreso);
    }
}