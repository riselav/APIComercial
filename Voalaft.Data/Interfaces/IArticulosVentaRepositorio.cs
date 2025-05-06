using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface IArticulosVentaRepositorio
    {
        Task<List<CatArticuloVenta>> ObtenArticulosVenta(ParametrosObtenArticulosVenta parametrosObtenArticulosVenta);
    }
}