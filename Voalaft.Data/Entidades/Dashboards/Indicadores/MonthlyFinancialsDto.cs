using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class MonthlyFinancialsDto
    {
        public string? Month { get; set; } // Ej. "Enero", "Febrero"
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }
    // Retornaría List<MonthlyFinancialsDto>
}
