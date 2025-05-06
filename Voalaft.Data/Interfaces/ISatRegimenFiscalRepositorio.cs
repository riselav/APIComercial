using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatRegimenFiscalRepositorio
    {
        Task<List<SatRegimenFiscal>> Lista();
        Task<SatRegimenFiscal> ObtenerPorId(long n_RegimenFiscal);
    }
}