using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class IncomeExpensesDetailDto
    {
        public List<IncomeExpenseChartData>? chartData { get; set; }
        public List<IncomeExpenseTableData>? tableData { get; set; }
    }
}