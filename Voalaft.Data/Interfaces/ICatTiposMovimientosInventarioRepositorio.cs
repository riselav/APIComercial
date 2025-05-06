using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatTiposMovimientosInventarioRepositorio
    {
        Task<List<CatTiposMovimientosInventario >> Lista();
        Task<CatTiposMovimientosInventario> ObtenerTipoMovimientoInventario(int nTipoMovtoInv);
        Task<CatTiposMovimientosInventario> IME_CAT_TiposMovimientosInventario(CatTiposMovimientosInventario  objTipoMovtoInv);
    }
}
