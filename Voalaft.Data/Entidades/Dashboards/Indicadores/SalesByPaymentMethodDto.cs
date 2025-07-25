using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class SalesByPaymentMethodDto
    {
        public string? PaymentMethod { get; set; }
        public decimal TotalSales { get; set; }
    }
    // Retornaría List<SalesByPaymentMethodDto>
}
