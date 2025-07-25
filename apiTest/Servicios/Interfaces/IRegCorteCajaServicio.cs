using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Consultas;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegCorteCajaServicio
    {
        Task<RegCorteCaja> IME_REG_CorteCaja(RegCorteCaja regCorteCaja);
        Task<List<ImpresionData>> TicketCorteCaja(int idSucursal, long idCorte);
    }
}