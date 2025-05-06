using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatCatUsoCFDIRepositorio
    {
        Task<List<SatCatUsoCFDI>> Lista();
        Task<SatCatUsoCFDI> ObtenerPorClave(string c_UsoCFDI);
    }
}