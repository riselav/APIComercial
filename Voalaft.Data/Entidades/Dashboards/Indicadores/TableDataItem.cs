using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TableDataItem
    {
        public string? category { get; set; }
        public double sales { get; set; }
        public int percentage { get; set; }
        public int products { get; set; }
        public double averageTicket { get; set; }
        public double trend { get; set; }
        public bool trendPositive { get; set; }
    }
}