using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class ReporteVentasDetalle
    {
        public int nIDArticulo { get; set; }
        public string cDescripcion { get; set; }
        public decimal nCantidad { get; set; }
        public decimal nPrecioUnitario { get; set; }
        public decimal nTotal { get; set; }        
    }
}
