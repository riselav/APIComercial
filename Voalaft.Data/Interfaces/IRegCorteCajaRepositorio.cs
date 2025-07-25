using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Consultas;

namespace Voalaft.Data.Interfaces
{
    public interface IRegCorteCajaRepositorio
    {
        Task<RegCorteCaja> IME_REG_CorteCaja(RegCorteCaja regCorteCaja);
        Task<List<ImpresionData>> TicketCorteCaja(int idSucursal, long idCorte);
    }
}