using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegAperturaCaja
    {
        public long IDApertura { get; set; }
        public int IDSucursal { get; set; }
        public int IDCaja { get; set; }
        public int IDTurno { get; set; }
        public DateTime Fecha { get; set; }
        public int? nFecha { get; set; }
        public decimal DotacionInicial { get; set; }
        public int IDEmpleado { get; set; }
        public int? IDUsuarioAutoriza { get; set; }
        public byte Estatus { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

        public RegMovimientoCaja? regMovimientoCaja { get; set; }
    }
}