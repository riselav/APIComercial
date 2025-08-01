using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class InvoicedUninvoicedDetailDto
    {
        public List<InvoicedUninvoicedChartData>? chartData { get; set; }
        public List<InvoicedUninvoicedTableData>? tableData { get; set; }
    }
}
