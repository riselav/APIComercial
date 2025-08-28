using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatConfiguracionTicketSucursal
    {
        public int Sucursal { get; set; }

        public List<string>? EncabezadoTicket { get; set; }

        public List<string>? PieTicket { get; set; }
    }
}
