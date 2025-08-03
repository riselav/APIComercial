using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class ServiceTableDataItem
    {
        public string? serviceType { get; set; }
        public double sales { get; set; }
        public double percentage { get; set; }
        public int orders { get; set; }
        public double averageTicket { get; set; }
        public double trend { get; set; }
        public bool trendPositive { get; set; }
    }
}