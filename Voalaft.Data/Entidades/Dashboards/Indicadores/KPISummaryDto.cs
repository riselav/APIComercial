using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class KPISummaryDto
    {
        public decimal totalSales { get; set; }
        public decimal totalSalesPreviousPeriod { get; set; } // Para calcular el porcentaje de cambio
        public decimal invoicedSales { get; set; }
        public decimal invoicedSalesPreviousPeriod { get; set; }
        public decimal uninvoicedSales { get; set; }
        public decimal uninvoicedSalesPreviousPeriod { get; set; }
        public decimal netIncome { get; set; }
        public decimal netIncomePreviousPeriod { get; set; }
        public decimal totalExpenses { get; set; }
        public decimal totalExpensesPreviousPeriod { get; set; }
        public int numberOfCustomers { get; set; }
        public int numberOfCustomersPreviousPeriod { get; set; }
        public decimal averageTicket { get; set; }
        public decimal averageTicketPreviousPeriod { get; set; }
        public decimal occupancyRate { get; set; } // Podría ser un porcentaje (0.78 para 78%)
        public decimal occupancyRatePreviousPeriod { get; set; }

        // Puedes añadir propiedades calculadas si no quieres que el frontend las calcule

        public string TotalSalesChangePercentage => CalculatePercentageChange(totalSales, totalSalesPreviousPeriod);

        public string InvoicedSalesChangePercentage => CalculatePercentageChange(invoicedSales, invoicedSalesPreviousPeriod);

        public string UnInvoicedSalesChangePercentage => CalculatePercentageChange(uninvoicedSales, uninvoicedSalesPreviousPeriod);

        public string NetIncomeChangePercentage => CalculatePercentageChange(netIncome, netIncomePreviousPeriod);

        public string TotalExpensesChangePercentage => CalculatePercentageChange(totalExpenses, totalExpensesPreviousPeriod);

        public string NumberOfCustomersChangePercentage => CalculatePercentageChange(numberOfCustomers, numberOfCustomersPreviousPeriod);

        public string AverageTicketChangePercentage => CalculatePercentageChange(averageTicket, averageTicketPreviousPeriod);

        public string OccupancyRateChangePercentage => CalculatePercentageChange(occupancyRate, occupancyRatePreviousPeriod);

        private string CalculatePercentageChange(decimal current, decimal previous)
        {
            if (previous == 0) return current > 0 ? "100.0%" : "0.0%"; // Manejo de división por cero
            decimal change = ((current - previous) / previous) * 100;
            return change.ToString("F1") + "%"; // Formato con un decimal
        }
    }
}
