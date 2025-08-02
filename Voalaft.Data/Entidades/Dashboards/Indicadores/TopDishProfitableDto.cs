using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TopDishProfitableDto
    {
        public string? dishName { get; set; }
        public decimal revenueOrProfit { get; set; } // Valor monetario
    }
    // Retornaría List<TopDishProfitableDto>
}
