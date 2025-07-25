using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class SalesByServiceTypeDto
    {
        public string? ServiceType { get; set; }
        public decimal TotalSales { get; set; }
    }
    // Retornaría List<SalesByServiceTypeDto>
}
