using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TableDataItemTopVendidos
    {
        public int ranking { get; set; }
        public string? product { get; set; }
        public string? category { get; set; }
        public int unitsSold { get; set; }
        public double unitPrice { get; set; }
        public double totalSales { get; set; }
        public double trend { get; set; }
        public bool trendPositive { get; set; }
    }
}