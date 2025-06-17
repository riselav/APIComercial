using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegMovimientoCaja
    {
        public long IDRegistroCaja { get; set; }
        public int TipoRegistroCaja { get; set; }
        public int Sucursal { get; set; }
        public long IDApertura { get; set; }
        public int? ConceptoCaja { get; set; }

        public string? Concepto { get; set; }

        public int? EmpleadoInvolucrado { get; set; }
        
        public int? Consecutivo { get; set; }

        public decimal Importe { get; set; }
        public short Efecto { get; set; }
        public DateTime Fecha { get; set; }
        public int? nFecha { get; set; }
        public string? Hora { get; set; }

        public string? UsuarioAutorizaRegistro { get; set; }
        public string? UsuarioAutorizaModificacion { get; set; }
        public string? UsuarioAutorizaCancelacion { get; set; }
        public long? IDCorteCaja { get; set; }

        public string? Observaciones { get; set; }

        public bool Activo { get; set; }

        public string? Usuario { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? Maquina { get; set; }

        public List<RegMovimientoCajaDetalle>? Detalle { get; set; }
    }
}