using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class MonthlyFinancialsDto
    {
        public string? month { get; set; } // Ej. "Enero", "Febrero"
        public decimal income { get; set; }
        public decimal expenses { get; set; }
    }
    // Retornaría List<MonthlyFinancialsDto>
}
