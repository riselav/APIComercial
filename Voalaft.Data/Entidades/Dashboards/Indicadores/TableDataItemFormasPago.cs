using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TableDataItemFormasPago
    {
        public string? paymentMethod { get; set; }
        public decimal sales { get; set; }
        public double percentage { get; set; }
        public int transactions { get; set; }
        public double averageTicket { get; set; }
        public double trend { get; set; }
        public bool trendPositive { get; set; }
    }
}
