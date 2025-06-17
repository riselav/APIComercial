using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class ImportesFormaPagoApertura
    {
        public int FormaPago { get; set; }
        public string? Descripcion { get; set; }
        public decimal ImporteSistema { get; set; }
        public decimal ImporteUsuario { get; set; }
        public decimal Diferencia { get; set; }
        public bool Efectivo { get; set; }
        
    }

}
