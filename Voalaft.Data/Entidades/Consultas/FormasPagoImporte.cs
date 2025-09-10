using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class FormasPagoImporte
    {
        public int FormaPago { get; set; }
        public string? Descripcion { get; set; }
        public decimal Importe { get; set; }
    }

}
