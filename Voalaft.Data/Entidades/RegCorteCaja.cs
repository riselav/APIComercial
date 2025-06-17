using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegCorteCaja
    {
        public long IDCorte { get; set; }
        public int IDSucursal { get; set; }
        public int IDCaja { get; set; }
        public DateTime Fecha { get; set; }
        public int? IDUsuarioAutoriza { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

        public List<RegCorteCajaDetalle>? listRegCorteCajaDetalle { get; set; }
        public List<RegDenominacionCorteCaja>? listRegDenominacionCorteCaja { get; set; }
    }
}