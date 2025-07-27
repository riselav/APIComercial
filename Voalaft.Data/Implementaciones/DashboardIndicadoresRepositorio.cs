using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Dashboards.Indicadores;
using Voalaft.Data.Entidades.Tableros;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class DashboardIndicadoresRepositorio:IDashboardIndicadoresRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<DashboardIndicadoresRepositorio> _logger;
   
        public DashboardIndicadoresRepositorio(ILogger<DashboardIndicadoresRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }
        public async Task<DashboardIndicadores> ObtenerDashboardIndicadores(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            DashboardIndicadores dashboardIndicadores = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_CON_ReporteIndicadores",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", n_Sucursal);
                    cmd.Parameters.AddWithValue("@FechaNumeroInicial", n_FechaInicial);
                    cmd.Parameters.AddWithValue("@FechaNumeroFinal", n_FechaFinal);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");
                        //int TipoPersonaIndex = reader.GetOrdinal("nTipoPersona");
                        //int IdRFCIndex = reader.GetOrdinal("nIDRFC");

                        dashboardIndicadores = new DashboardIndicadores();

                        while (await reader.ReadAsync())
                        {
                            dashboardIndicadores.ResumenIndicadores =
                                new KPISummaryDto()
                                {
                                    TotalSales = ConvertUtils.ToDecimal(reader["totalSales"]),
                                    TotalSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["totalSalesPreviousPeriod"]),
                                    InvoicedSales = ConvertUtils.ToDecimal(reader["invoicedSales"]),
                                    InvoicedSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["invoicedSalesPreviousPeriod"]),
                                    UninvoicedSales = ConvertUtils.ToDecimal(reader["uninvoicedSales"]),
                                    UninvoicedSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["uninvoicedSalesPreviousPeriod"]),
                                    NetIncome = ConvertUtils.ToDecimal(reader["netIncome"]),
                                    NetIncomePreviousPeriod = ConvertUtils.ToDecimal(reader["netIncomePreviousPeriod"]),
                                    TotalExpenses = ConvertUtils.ToDecimal(reader["totalExpenses"]),
                                    TotalExpensesPreviousPeriod = ConvertUtils.ToDecimal(reader["totalExpensesPreviousPeriod"]),
                                    //nTipoPersona = reader.IsDBNull(TipoPersonaIndex) ? 0 : reader.GetInt32(TipoPersonaIndex),
                                    //nIDRFC = reader.IsDBNull(IdRFCIndex) ? 0 : reader.GetInt64(IdRFCIndex),

                                    NumberOfCustomers = ConvertUtils.ToInt32(reader["numberOfCustomers"]),
                                    NumberOfCustomersPreviousPeriod = ConvertUtils.ToInt32(reader["NumberOfCustomersPreviousPeriod"]),
                                    AverageTicket = ConvertUtils.ToDecimal(reader["averageTicket"]),
                                    AverageTicketPreviousPeriod = ConvertUtils.ToDecimal(reader["averageTicketPreviousPeriod"]),
                                    OccupancyRate = ConvertUtils.ToDecimal(reader["occupancyRate"]),
                                    OccupancyRatePreviousPeriod = ConvertUtils.ToDecimal(reader["occupancyRatePreviousPeriod"])
                                };


                            // Mover al siguiente resultado (Ventas Diarias)
                            dashboardIndicadores.VentasDiarias = new List<DailySalesDto>();
                            dashboardIndicadores.VentasDiarias = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasDiarias.Add(new DailySalesDto
                                    {
                                        Date = Convert.ToString(reader["Date"]),
                                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas por Categoria)
                            dashboardIndicadores.VentasPorCategoria = new List<SalesByCategoryDto>();
                            dashboardIndicadores.VentasPorCategoria = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasPorCategoria.Add(new SalesByCategoryDto
                                    {
                                        CategoryName = Convert.ToString(reader["CategoryName"]),
                                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ingresos Vs Gastos)
                            dashboardIndicadores.IngresosVsGastos = new List<MonthlyFinancialsDto>();
                            dashboardIndicadores.IngresosVsGastos = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.IngresosVsGastos.Add(new MonthlyFinancialsDto
                                    {
                                        Month = Convert.ToString(reader["mes"]),
                                        Income = Convert.ToDecimal(reader["income"]),
                                        Expenses = Convert.ToDecimal(reader["expenses"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Facturado Vs No Facturado)
                            dashboardIndicadores.VentasFacturadasVsNoFacturadas = new List<MonthlyInvoiceStatusDto>();
                            dashboardIndicadores.VentasFacturadasVsNoFacturadas = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasFacturadasVsNoFacturadas.Add(new MonthlyInvoiceStatusDto
                                    {
                                        Month = Convert.ToString(reader["mes"]),
                                        InvoicedSales = Convert.ToDecimal(reader["invoiced"]),
                                        UninvoicedSales = Convert.ToDecimal(reader["uninvoiced"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Forma de Pago)
                            dashboardIndicadores.VentasPorFormaPago = new List<SalesByPaymentMethodDto>();
                            dashboardIndicadores.VentasPorFormaPago = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasPorFormaPago.Add(new SalesByPaymentMethodDto
                                    {
                                        PaymentMethod = Convert.ToString(reader["PaymentMethod"]),
                                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Tipo de Servicio)
                            dashboardIndicadores.VentasPorTipoServicio = new List<SalesByServiceTypeDto>();
                            dashboardIndicadores.VentasPorTipoServicio = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasPorTipoServicio.Add(new SalesByServiceTypeDto
                                    {
                                        ServiceType = Convert.ToString(reader["ServiceType"]),
                                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Estacion de Cocina)
                            dashboardIndicadores.VentasPorEstacionCocina = new List<SalesByKitchenStationDto>();
                            dashboardIndicadores.VentasPorEstacionCocina = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.VentasPorEstacionCocina.Add(new SalesByKitchenStationDto
                                    {
                                        StationName = Convert.ToString(reader["StationName"]),
                                        TotalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Top 10 Platillos Mas Vendidos)
                            dashboardIndicadores.topPlatillosMasVendidos = new List<TopDishDto>();
                            dashboardIndicadores.topPlatillosMasVendidos = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.topPlatillosMasVendidos.Add(new TopDishDto
                                    {
                                        DishName = Convert.ToString(reader["DishName"]),
                                        Quantity = Convert.ToInt32(reader["Quantity"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Top 10 Platillos Mas Redituables)
                            dashboardIndicadores.topPlatillosMasRentables = new List<TopDishProfitableDto>();
                            dashboardIndicadores.topPlatillosMasRentables = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.topPlatillosMasRentables.Add(new TopDishProfitableDto
                                    {
                                        DishName = Convert.ToString(reader["DishName"]),
                                        RevenueOrProfit = Convert.ToDecimal(reader["RevenueOrProfit"])
                                    }
                                    );
                                }
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener reporte de Indicadores")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return dashboardIndicadores;
        }
    }
}
