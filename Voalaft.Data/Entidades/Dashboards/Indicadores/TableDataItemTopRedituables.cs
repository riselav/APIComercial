using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TableDataItemTopRedituables
    {
        public int ranking { get; set; }
        public string? product { get; set; }
        public string? category { get; set; }
        public double totalSales { get; set; }
        public double cost { get; set; }
        public double profit { get; set; }
        public double margin { get; set; }           // Porcentaje
        public double trend { get; set; }         // Porcentaje de cambio
        public bool trendPositive { get; set; }
    }
}