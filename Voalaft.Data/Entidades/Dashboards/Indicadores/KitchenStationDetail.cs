using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class KitchenStationDetail
    {
        public List<KitchenChartDataItem>? chartData { get; set; }
        public List<KitchenTableDataItem>? tableData { get; set; }
    }
}