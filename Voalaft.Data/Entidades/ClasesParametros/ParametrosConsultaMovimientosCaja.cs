using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.ClasesParametros
{
    public class ParametrosConsultaMovimientosCaja
    {
        //@nSucursal int,@nCaja int,@nTipoRegistroCaja int

        public int? Sucursal { get; set; }
        public int? Caja { get; set; }

        public int? TipoRegistroCaja { get; set; }

        public long? IDApertura { get; set; }
    }
}