using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class ContactoCliente
    {
        public long cliente { get; set; }

        public int contacto { get; set; } // smallint en SQL es short en C#

        public string nombre { get; set; } = string.Empty; // NOT NULL en SQL, inicializado

        public string puesto { get; set; } = string.Empty; // NOT NULL

        public string? telefono { get; set; } // NULL en SQL es string? en C#

        public string celular { get; set; } = string.Empty; // NOT NULL

        public string correoElectronico { get; set; } = string.Empty; // NOT NULL

        public int tipoContacto { get; set; } // tinyint en SQL es byte en C#

        public string? descripcionTipoContacto { get; set; } // tinyint en SQL es byte en C#

        public bool activo { get; set; } // bit en SQL es bool en C#

        public string usuario { get; set; } = string.Empty; // NOT NULL

        public string maquina { get; set; } = string.Empty; // NOT NULL
    }
}
