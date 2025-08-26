using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class ReporteVentas
    {
        public long folio { get; set; }
        public long nVenta { get; set; }
        public int nTipoRegistro { get; set; }
        public string cTipoRegistro { get; set; }
        public DateTime fechaHora { get; set; }
        public int nEmpleadoRegistra { get; set; }
        public string cajero { get; set; }
        public int? nCliente { get; set; } 
        public string cNombreCompleto { get; set; }
        public decimal importe { get; set; }
        public long nFactura { get; set; }
        public string cComentarios { get; set; }
        public bool bActivo { get; set; }
        public List<ReporteVentasDetalle> listReporteVentasDetalle { get; set; }
    }
}
