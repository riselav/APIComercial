using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatCorreoContactoRFC
    {
        public long IDRFC { get; set; }

        public int Folio { get; set; }

        public string? CorreoElectronico{ get; set; }
    
        public bool? Activo { get; set;}

        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
