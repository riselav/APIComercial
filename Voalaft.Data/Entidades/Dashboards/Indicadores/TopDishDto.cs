using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TopDishDto
    {
        public string? dishName { get; set; }
        public int quantity { get; set; } // O decimal si es por volumen
    }
    // Retornaría List<TopDishDto>
}
