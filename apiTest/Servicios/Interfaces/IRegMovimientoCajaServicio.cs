using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegMovimientoCajaServicio
    {
        Task<RegMovimientoCaja> IME_REG_MovimientoCaja(RegMovimientoCaja regMovimientoCaja);
    }
}