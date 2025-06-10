using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;

namespace Voalaft.Data.Interfaces
{
    public interface IRegMovimientoCajaRepositorio
    {
        Task<RegMovimientoCaja> IME_REG_MovimientoCaja(RegMovimientoCaja regMovimientoCaja,
                SqlConnection externalConnection = null,
                SqlTransaction externalTransaction = null);
        Task<List<RegMovimientoCaja>> ObtenMovimientosCaja(ParametrosConsultaMovimientosCaja parametros);
        //Task<Decimal> ObtenImporteDisponibleCaja(ParametrosConsultaMovimientosCaja parametros);
    }
}