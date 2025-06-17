using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegCorteCajaDetalle : ImportesFormaPagoApertura
    {
        public long IDCorte { get; set; }
        public int Renglon { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

    }
}