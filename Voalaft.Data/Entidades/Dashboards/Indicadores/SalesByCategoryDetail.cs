using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class SalesByCategoryDetail
    {
        public List<ChartDataItem>? chartData { get; set; }
        public List<TableDataItem>? tableData { get; set; }
    }
}