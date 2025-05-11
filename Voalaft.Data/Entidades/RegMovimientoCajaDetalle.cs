using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegMovimientoCajaDetalle
    {
        public long IDRegistroCaja { get; set; }
        public int Renglon { get; set; }
        public int FormaPago { get; set; }
        public decimal Importe { get; set; }
             
        public long? Orden { get; set; }
        public long? Cuenta { get; set; }
        public string? Referencia { get; set; }
        public string? ReferenciaCuponVale { get; set; }
        public decimal? ImporteFacturado { get; set; }
        public decimal? ImportePropina { get; set; }
        public int? Cliente { get; set; }
        public int? Empleado { get; set; }
        public decimal? Propina { get; set; }
        public decimal? Cambio { get; set; }
        public decimal? PagaCon { get; set; }

        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

        public List<RegMovimientoCajaDetalleDenominacion>? DetalleDenominacion { get; set; }
    }
}