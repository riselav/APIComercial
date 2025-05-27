using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatFormaPago
    {
        public int FormaPago { get; set; }
        public string? Descripcion { get; set; }
        public bool Ingreso { get; set; }

        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }   
    }

}
