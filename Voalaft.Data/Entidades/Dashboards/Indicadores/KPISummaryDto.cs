using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class KPISummaryDto
    {
        public decimal TotalSales { get; set; }
        public decimal TotalSalesPreviousPeriod { get; set; } // Para calcular el porcentaje de cambio
        public decimal InvoicedSales { get; set; }
        public decimal InvoicedSalesPreviousPeriod { get; set; }
        public decimal UninvoicedSales { get; set; }
        public decimal UninvoicedSalesPreviousPeriod { get; set; }
        public decimal NetIncome { get; set; }
        public decimal NetIncomePreviousPeriod { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalExpensesPreviousPeriod { get; set; }
        public int NumberOfCustomers { get; set; }
        public int NumberOfCustomersPreviousPeriod { get; set; }
        public decimal AverageTicket { get; set; }
        public decimal AverageTicketPreviousPeriod { get; set; }
        public decimal OccupancyRate { get; set; } // Podría ser un porcentaje (0.78 para 78%)
        public decimal OccupancyRatePreviousPeriod { get; set; }

        // Puedes añadir propiedades calculadas si no quieres que el frontend las calcule

        public string TotalSalesChangePercentage => CalculatePercentageChange(TotalSales, TotalSalesPreviousPeriod);

        public string InvoicedSalesChangePercentage => CalculatePercentageChange(InvoicedSales, InvoicedSalesPreviousPeriod);

        public string UnInvoicedSalesChangePercentage => CalculatePercentageChange(UninvoicedSales, UninvoicedSalesPreviousPeriod);

        public string NetIncomeChangePercentage => CalculatePercentageChange(NetIncome, NetIncomePreviousPeriod);

        public string TotalExpensesChangePercentage => CalculatePercentageChange(TotalExpenses, TotalExpensesPreviousPeriod);

        public string NumberOfCustomersChangePercentage => CalculatePercentageChange(NumberOfCustomers, NumberOfCustomersPreviousPeriod);

        public string AverageTicketChangePercentage => CalculatePercentageChange(AverageTicket, AverageTicketPreviousPeriod);

        public string OccupancyRateChangePercentage => CalculatePercentageChange(OccupancyRate, OccupancyRatePreviousPeriod);

        private string CalculatePercentageChange(decimal current, decimal previous)
        {
            if (previous == 0) return current > 0 ? "100.0%" : "0.0%"; // Manejo de división por cero
            decimal change = ((current - previous) / previous) * 100;
            return change.ToString("F1") + "%"; // Formato con un decimal
        }
    }
}
