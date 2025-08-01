using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class InvoicedUninvoicedChartData
    {
        public string? name { get; set; }                 // Ej: "Enero"
        public decimal ventasFacturadas { get; set; }     // Ej: 7687.50
        public decimal ventasNoFacturadas { get; set; }   // Ej: 2562.50
    }
}
