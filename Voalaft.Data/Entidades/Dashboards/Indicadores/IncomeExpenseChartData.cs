using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class IncomeExpenseChartData
    {
        public string? name { get; set; }          // Ej: "Enero"
        public decimal ingresos { get; set; }     // Ej: 10250
        public decimal gastos { get; set; }       // Ej: 3850
        public decimal gananciaNeta { get; set; } // Ej: 6400
    }
}