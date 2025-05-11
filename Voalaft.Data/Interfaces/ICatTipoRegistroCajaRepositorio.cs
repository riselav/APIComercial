using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatTipoRegistroCajaRepositorio
    {
        Task<List<CatTipoRegistroCaja>> Lista();
        Task<CatTipoRegistroCaja> ObtenerPorId(int n_TipoRegistroCaja);
    }
}