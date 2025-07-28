using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class MonthlyInvoiceStatusDto
    {
        public string? month { get; set; }
        public decimal invoicedSales { get; set; }
        public decimal uninvoicedSales { get; set; }
    }
    // Retornaría List<MonthlyInvoiceStatusDto>
}
