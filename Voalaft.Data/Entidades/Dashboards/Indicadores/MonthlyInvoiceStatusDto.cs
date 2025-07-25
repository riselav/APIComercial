using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class MonthlyInvoiceStatusDto
    {
        public string? Month { get; set; }
        public decimal InvoicedSales { get; set; }
        public decimal UninvoicedSales { get; set; }
    }
    // Retornaría List<MonthlyInvoiceStatusDto>
}
