using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class IncomeExpenseTableData
    {
        public string? month { get; set; }             // Ej: "Enero"
        public decimal income { get; set; }           // Ej: 10250
        public decimal expenses { get; set; }         // Ej: 3850
        public decimal netProfit { get; set; }        // Ej: 6400
        public decimal margin { get; set; }           // Ej: 62.4 (porcentaje)
        public decimal trend { get; set; }            // Ej: 5.2 (valor de tendencia)
        public bool trendPositive { get; set; }       // Ej: true o false
    }
}