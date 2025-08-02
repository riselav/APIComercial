using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class InvoicedUninvoicedTableData
    {
        public string? month { get; set; }                   // Ej: "Enero"
        public decimal totalSales { get; set; }              // Ej: 10250.00
        public decimal invoicedSales { get; set; }           // Ej: 7687.50
        public decimal invoicedPercentage { get; set; }      // Ej: 75
        public decimal uninvoicedSales { get; set; }         // Ej: 2562.50
        public decimal uninvoicedPercentage { get; set; }    // Ej: 25
    }
}