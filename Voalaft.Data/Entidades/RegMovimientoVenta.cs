using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegMovimientoVenta
    {
        public long nVenta { get; set; }
        public byte nTipoRegistro { get; set; }
        public byte? nTipoVenta { get; set; }
        public int nSucursal { get; set; }
        public int? nCaja { get; set; }
        public long nCliente { get; set; }
        public int? nIdLista { get; set; }
        public long? nIDApertura { get; set; }
        public DateTime dFecha { get; set; }
        public int? nFecha { get; set; }
        public int nConsecutivo { get; set; }
        public long? nCotizacion { get; set; }
        public long? nVentaOrigenDevolucion { get; set; }
        public int nEmpleadoRegistra { get; set; }
        public decimal nSubtotal { get; set; }
        public decimal nImpuestoIVA { get; set; }
        public decimal? nImpuestoIEPS { get; set; }
        public decimal nImporteDescuento { get; set; }
        public decimal? nPorcentajeDescuento { get; set; }
        public decimal nTotal { get; set; }
        public long? nIDRegistroCaja { get; set; }
        public decimal? nPagaCon { get; set; }
        public decimal nCambio { get; set; }
        public bool bFactura { get; set; }
        public decimal? nImporteFactura { get; set; }
        public int? nFacturaFinDia { get; set; }
        public int? nFactura { get; set; }
        public string? cComentarios { get; set; }
        public string? cNombreCliente { get; set; }
        public int? nEmpleadoCancela { get; set; }
        public int? nEmpleadoAutorizaCancelacion { get; set; }
        public int? nMotivoCancelacion { get; set; }
        public string? cObservacionesCancelacion { get; set; }
        public bool bActivo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

        public List<RegMovimientoVentaDetalle>? Detalle { get; set; }

        public RegMovimientoCaja? regMovimientoCaja { get; set; }
    }
}