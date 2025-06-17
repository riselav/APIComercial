using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegMovimientoCajaServicio
    {
        Task<RegMovimientoCaja> IME_REG_MovimientoCaja(RegMovimientoCaja regMovimientoCaja);
        Task<List<RegMovimientoCaja>> ObtenMovimientosCaja(ParametrosConsultaMovimientosCaja parametros);

        Task<Decimal> ObtenImporteDisponibleCaja(ParametrosConsultaMovimientosCaja parametros);
        Task<Int32> CancelarMovimientoCaja(ParametrosCancelarMovimientoCaja parametros);
    }
}