using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class MovimientoVentaDet
    {
        public long nVenta { get; set; } 
        public int nRenglon { get; set; } 
        public int nIDArticulo { get; set; }
        public string? cDescripcion { get; set; }        
        public decimal nCantidad { get; set; } 
        public decimal nCantidadDevuelta { get; set; } 
        public decimal nPrecioUnitario { get; set; } 
        public decimal nPrecioOriginal { get; set; } 
        public decimal nSubtotal { get; set; } 
        public decimal nImpuestoIVA { get; set; }
        public int nIDImpuestoIVA { get; set; }
        public decimal nPorcentajeImpuestoIVA { get; set; } 
        public decimal nImpuestoIEPS { get; set; }
        public int nIDImpuestoIEPS { get; set; }
        public decimal nPorcentajeImpuestoIEPS { get; set; } 
        public decimal nImporteDescuento { get; set; } 
        public decimal nPorcentajeDescuento { get; set; } 
        public decimal nTotal { get; set; } 
        public string? cComentarios { get; set; }
    }
}
