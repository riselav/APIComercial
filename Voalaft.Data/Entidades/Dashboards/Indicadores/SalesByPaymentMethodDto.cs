using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class SalesByPaymentMethodDto
    {
        public string? paymentMethod { get; set; }
        public decimal totalSales { get; set; }
    }
    // Retornaría List<SalesByPaymentMethodDto>
}
