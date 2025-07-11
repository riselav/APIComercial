using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class MovimientoVentaEnc
    {
        public long nVenta { get; set; } 
        public int nTipoRegistro { get; set; }
        public int nTipoVenta { get; set; }
        public int nSucursal { get; set; }
        public string cDescripcion { get; set; } 
        public int nCaja { get; set; }
        public int? nCliente { get; set; } 
        public string cNombreCompleto { get; set; } 
        public long nIDApertura { get; set; } 
        public long nConsecutivo { get; set; } 
        public decimal nSubtotal { get; set; }
        public decimal nImpuestoIVA { get; set; }
        public decimal nImpuestoIEPS { get; set; }
        public decimal nImporteDescuento { get; set; }
        public decimal nTotal { get; set; }
        public string cComentarios { get; set; }
        public List<MovimientoVentaDet> listMovimientoVentaDet { get; set; }
    }
}
