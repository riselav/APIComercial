using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class ServiceTypeDetail
    {
        public List<ServiceChartDataItem>? chartData { get; set; }
        public List<ServiceTableDataItem>? tableData { get; set; }
    }
}