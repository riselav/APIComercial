using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class ImpresionTicketData
    {
        public long nVenta { get; set; } 
        public List<string> LineasImprimibles { get; set; }        
    }
}
