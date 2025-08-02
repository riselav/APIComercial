using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Dashboards.Indicadores
{
    public class DashboardIndicadores
    {
        public KPISummaryDto? resumenIndicadores { get; set; }

        public List<DailySalesDto>? ventasDiarias { get; set; }

        public List<SalesByCategoryDto>? ventasPorCategoria { get; set; }

        public List<MonthlyFinancialsDto>? ingresosVsGastos { get; set; }

        public List<MonthlyInvoiceStatusDto>? ventasFacturadasVsNoFacturadas { get; set; }  
    
        public List<SalesByPaymentMethodDto>? ventasPorFormaPago { get; set; }

        public List<SalesByServiceTypeDto>? ventasPorTipoServicio { get; set; }

        public List<SalesByKitchenStationDto>? ventasPorEstacionCocina { get; set; }

        public List<TopDishDto>? topPlatillosMasVendidos { get; set; }

        public List<TopDishProfitableDto>? topPlatillosMasRentables { get; set; }
    }
}
