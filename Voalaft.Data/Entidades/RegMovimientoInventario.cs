using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegMovimientoInventario
    {
        public long NFolioMovimiento { get; set; }
        public int NAlmacen { get; set; }
        public int NTipoMovimiento { get; set; }
        public short NEfecto { get; set; }
        public DateTime DFecha { get; set; }
        public int? NFecha { get; set; }
        public string CReferencia { get; set; }
        public long? NFolioSolicitud { get; set; }
        public long? NFolioMateriaPrima { get; set; }
        public int? NAlmacenRelacionado { get; set; }
        public long? NFolioMovimientoRelacionado { get; set; }
        public long? NFolioMovimientoCancelacion { get; set; }
        public byte NTipoInvolucrado { get; set; }
        public int NIDInvolucrado { get; set; }
        public bool BAplicado { get; set; }
        public bool BAplicadoParcial { get; set; }
        public bool BTraspaso { get; set; }
        public byte NEstatus { get; set; }
        public long? NIDReferencia { get; set; }
        public long? NIDFactura { get; set; }
        public long? NIDFacturaTraslado { get; set; }
        public bool BActivo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
