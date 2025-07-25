using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class DashboardIndicadores
    {
        public KPISummaryDto? ResumenIndicadores { get; set; }

        public List<DailySalesDto>? VentasDiarias { get; set; }

        public List<SalesByCategoryDto>? VentasPorCategoria { get; set; }

        public List<MonthlyFinancialsDto>? IngresosVsGastos { get; set; }

        public List<MonthlyInvoiceStatusDto>? VentasFacturadasVsNoFacturadas { get; set; }  
    
        public List<SalesByPaymentMethodDto>? VentasPorFormaPago { get; set; }

        public List<SalesByServiceTypeDto>? VentasPorTipoServicio { get; set; }

        public List<SalesByKitchenStationDto>? VentasPorEstacionComida { get; set; }

        public List<TopDishDto>? topPlatillosMasVendidos { get; set; }

        public List<TopDishProfitableDto>? topPlatillosMasRentables { get; set; }
    }
}
