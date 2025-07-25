using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class TopDishDto
    {
        public string? DishName { get; set; }
        public int Quantity { get; set; } // O decimal si es por volumen
    }
    // Retornaría List<TopDishDto>
}
