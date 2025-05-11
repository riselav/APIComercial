using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegMovimientoCajaDetalleDenominacion
    {
        public long IDRegistroCaja { get; set; }
        public int Renglon { get; set; }
        public int Denominacion { get; set; }
        public decimal Valor { get; set; }
        public int Cantidad { get; set; }
        public decimal Importe { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }

}
