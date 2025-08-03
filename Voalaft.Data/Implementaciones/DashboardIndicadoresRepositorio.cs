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
                            dashboardIndicadores.resumenIndicadores =
                                new KPISummaryDto()
                                {
                                    totalSales = ConvertUtils.ToDecimal(reader["totalSales"]),
                                    totalSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["totalSalesPreviousPeriod"]),
                                    invoicedSales = ConvertUtils.ToDecimal(reader["invoicedSales"]),
                                    invoicedSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["invoicedSalesPreviousPeriod"]),
                                    uninvoicedSales = ConvertUtils.ToDecimal(reader["uninvoicedSales"]),
                                    uninvoicedSalesPreviousPeriod = ConvertUtils.ToDecimal(reader["uninvoicedSalesPreviousPeriod"]),
                                    netIncome = ConvertUtils.ToDecimal(reader["netIncome"]),
                                    netIncomePreviousPeriod = ConvertUtils.ToDecimal(reader["netIncomePreviousPeriod"]),
                                    totalExpenses = ConvertUtils.ToDecimal(reader["totalExpenses"]),
                                    totalExpensesPreviousPeriod = ConvertUtils.ToDecimal(reader["totalExpensesPreviousPeriod"]),
                                    //nTipoPersona = reader.IsDBNull(TipoPersonaIndex) ? 0 : reader.GetInt32(TipoPersonaIndex),
                                    //nIDRFC = reader.IsDBNull(IdRFCIndex) ? 0 : reader.GetInt64(IdRFCIndex),

                                    numberOfCustomers = ConvertUtils.ToInt32(reader["numberOfCustomers"]),
                                    numberOfCustomersPreviousPeriod = ConvertUtils.ToInt32(reader["NumberOfCustomersPreviousPeriod"]),
                                    averageTicket = ConvertUtils.ToDecimal(reader["averageTicket"]),
                                    averageTicketPreviousPeriod = ConvertUtils.ToDecimal(reader["averageTicketPreviousPeriod"]),
                                    occupancyRate = ConvertUtils.ToDecimal(reader["occupancyRate"]),
                                    occupancyRatePreviousPeriod = ConvertUtils.ToDecimal(reader["occupancyRatePreviousPeriod"])
                                };


                            // Mover al siguiente resultado (Ventas Diarias)
                            dashboardIndicadores.ventasDiarias = new List<DailySalesDto>();
                            //dashboardIndicadores.ventasDiarias = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasDiarias.Add(new DailySalesDto
                                    {
                                        date = Convert.ToString(reader["Date"]),
                                        totalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas por Categoria)
                            dashboardIndicadores.ventasPorCategoria = new List<SalesByCategoryDto>();
                            //dashboardIndicadores.ventasPorCategoria = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasPorCategoria.Add(new SalesByCategoryDto
                                    {
                                        categoryName = Convert.ToString(reader["CategoryName"]),
                                        totalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ingresos Vs Gastos)
                            dashboardIndicadores.ingresosVsGastos = new List<MonthlyFinancialsDto>();
                            //dashboardIndicadores.ingresosVsGastos = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ingresosVsGastos.Add(new MonthlyFinancialsDto
                                    {
                                        month = Convert.ToString(reader["mes"]),
                                        income = Convert.ToDecimal(reader["income"]),
                                        expenses = Convert.ToDecimal(reader["expenses"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Facturado Vs No Facturado)
                            dashboardIndicadores.ventasFacturadasVsNoFacturadas = new List<MonthlyInvoiceStatusDto>();
                            //dashboardIndicadores.ventasFacturadasVsNoFacturadas = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasFacturadasVsNoFacturadas.Add(new MonthlyInvoiceStatusDto
                                    {
                                        month = Convert.ToString(reader["mes"]),
                                        invoicedSales = Convert.ToDecimal(reader["invoiced"]),
                                        uninvoicedSales = Convert.ToDecimal(reader["uninvoiced"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Forma de Pago)
                            dashboardIndicadores.ventasPorFormaPago = new List<SalesByPaymentMethodDto>();
                            //dashboardIndicadores.ventasPorFormaPago = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasPorFormaPago.Add(new SalesByPaymentMethodDto
                                    {
                                        paymentMethod = Convert.ToString(reader["PaymentMethod"]),
                                        totalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Tipo de Servicio)
                            dashboardIndicadores.ventasPorTipoServicio = new List<SalesByServiceTypeDto>();
                            //dashboardIndicadores.ventasPorTipoServicio = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasPorTipoServicio.Add(new SalesByServiceTypeDto
                                    {
                                        serviceType = Convert.ToString(reader["ServiceType"]),
                                        totalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Ventas Por Estacion de Cocina)
                            dashboardIndicadores.ventasPorEstacionCocina = new List<SalesByKitchenStationDto>();
                            //dashboardIndicadores.ventasPorEstacionCocina = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.ventasPorEstacionCocina.Add(new SalesByKitchenStationDto
                                    {
                                        stationName = Convert.ToString(reader["StationName"]),
                                        totalSales = Convert.ToDecimal(reader["TotalSales"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Top 10 Platillos Mas Vendidos)
                            dashboardIndicadores.topPlatillosMasVendidos = new List<TopDishDto>();
                            //dashboardIndicadores.topPlatillosMasVendidos = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.topPlatillosMasVendidos.Add(new TopDishDto
                                    {
                                        dishName = Convert.ToString(reader["DishName"]),
                                        quantity = Convert.ToInt32(reader["Quantity"])
                                    }
                                    );
                                }
                            }

                            // Mover al siguiente resultado (Top 10 Platillos Mas Redituables)
                            dashboardIndicadores.topPlatillosMasRentables = new List<TopDishProfitableDto>();
                            //dashboardIndicadores.topPlatillosMasRentables = [];

                            if (reader.NextResult())
                            {
                                //int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                while (reader.Read())
                                {

                                    dashboardIndicadores?.topPlatillosMasRentables.Add(new TopDishProfitableDto
                                    {
                                        dishName = Convert.ToString(reader["DishName"]),
                                        revenueOrProfit = Convert.ToDecimal(reader["RevenueOrProfit"])
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
                    Metodo = "ObtenerDashboardIndicadores",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return dashboardIndicadores;
        }

        public async Task<SalesByCategoryDetail> ObtenerDetalleVentasPorCategoria(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            SalesByCategoryDetail detalleVentasPorCategoria = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 1);
                    detalleVentasPorCategoria = new SalesByCategoryDetail();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleVentasPorCategoria.chartData = new List<ChartDataItem>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleVentasPorCategoria.chartData.Add(new ChartDataItem
                            {
                                name =  Convert.ToString(reader["name"]),
                                value = Convert.ToDouble(reader["valor"])
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleVentasPorCategoria.tableData = new List<TableDataItem>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleVentasPorCategoria.tableData.Add(new TableDataItem
                                {
                                    category = reader["category"].ToString(),
                                    sales = Convert.ToDouble(reader["sales"]),
                                    percentage = Convert.ToInt32(reader["porcentaje"]),
                                    products = Convert.ToInt32(reader["products"]),
                                    averageTicket = Convert.ToDouble(reader["averageTicket"]),
                                    trend = Convert.ToDouble(reader["trend"]),
                                    trendPositive = Convert.ToBoolean(reader["trendPositive"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle ventas por categoría")
                {
                    Metodo = "ObtenerDetalleVentasPorCategoria",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleVentasPorCategoria;
        }

        public async Task<IncomeExpensesDetailDto> ObtenerDetalleIngresosVsGastos(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            IncomeExpensesDetailDto detalleIngresosVsGastos = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 2);
                    detalleIngresosVsGastos = new IncomeExpensesDetailDto();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleIngresosVsGastos.chartData = new List<IncomeExpenseChartData>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleIngresosVsGastos.chartData.Add(new IncomeExpenseChartData
                            {
                                name = Convert.ToString(reader["mes"]),
                                ingresos = Convert.ToDecimal(reader["income"]),
                                gastos = Convert.ToDecimal(reader["expenses"]),
                                gananciaNeta = Convert.ToDecimal(reader["gananciaNeta"]),
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleIngresosVsGastos.tableData = new List<IncomeExpenseTableData>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleIngresosVsGastos.tableData.Add(new IncomeExpenseTableData
                                {
                                    month = reader["mes"].ToString(),
                                    income = Convert.ToDecimal(reader["income"]),
                                    expenses = Convert.ToDecimal(reader["expenses"]),
                                    netProfit = Convert.ToDecimal(reader["netProfit"]),
                                    margin = Convert.ToDecimal(reader["margin"]),
                                    trend = Convert.ToDecimal(reader["trend"]),
                                    trendPositive = Convert.ToBoolean(reader["trendPositive"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle de ingresos vs gastos")
                {
                    Metodo = "ObtenerDetalleIngresosVsGastos",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleIngresosVsGastos;
        }

        public async Task<InvoicedUninvoicedDetailDto> ObtenerDetalleFacturadoVsNoFacturado(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            InvoicedUninvoicedDetailDto detalleFacturadoVsNoFacturado = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 3);
                    detalleFacturadoVsNoFacturado = new InvoicedUninvoicedDetailDto();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleFacturadoVsNoFacturado.chartData = new List<InvoicedUninvoicedChartData>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleFacturadoVsNoFacturado.chartData.Add(new InvoicedUninvoicedChartData
                            {
                                name = Convert.ToString(reader["mes"]),
                                ventasFacturadas = Convert.ToDecimal(reader["ventasFacturadas"]),
                                ventasNoFacturadas = Convert.ToDecimal(reader["ventasNoFacturadas"])
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleFacturadoVsNoFacturado.tableData = new List<InvoicedUninvoicedTableData>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleFacturadoVsNoFacturado.tableData.Add(new InvoicedUninvoicedTableData
                                {
                                    month = reader["mes"].ToString(),
                                    totalSales = Convert.ToDecimal(reader["ventaTotal"]),
                                    invoicedSales = Convert.ToDecimal(reader["ventasFacturadas"]),
                                    invoicedPercentage = Convert.ToDecimal(reader["invoicedPercentage"]),
                                    uninvoicedSales = Convert.ToDecimal(reader["ventasNoFacturadas"]),
                                    uninvoicedPercentage = Convert.ToDecimal(reader["uninvoicedPercentage"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle de Facturado vs No Facturado")
                {
                    Metodo = "ObtenerDetalleFacturadoVsNoFacturado",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleFacturadoVsNoFacturado;
        }

        public async Task<PaymentMethodDetail> ObtenerDetalleVentaPorFormaPago(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            PaymentMethodDetail detalleVentaPorFormaPago = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 4);
                    detalleVentaPorFormaPago = new PaymentMethodDetail();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleVentaPorFormaPago.chartData = new List<ChartDataItem>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleVentaPorFormaPago.chartData.Add(new ChartDataItem
                            {
                                name = Convert.ToString(reader["PaymentMethod"]),
                                value = Convert.ToDouble(reader["TotalSales"])
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleVentaPorFormaPago.tableData = new List<TableDataItemFormasPago>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleVentaPorFormaPago.tableData.Add(new TableDataItemFormasPago
                                {
                                    paymentMethod = reader["PaymentMethod"].ToString(),
                                    sales = Convert.ToDecimal(reader["sales"]),
                                    percentage = Convert.ToDouble(reader["porcentaje"]),
                                    transactions = Convert.ToInt32(reader["transactions"]),
                                    averageTicket = Convert.ToDouble(reader["averageTicket"]),
                                    trend = Convert.ToDouble(reader["trend"]),
                                    trendPositive= Convert.ToBoolean(reader["trendPositive"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle de Venta por forma de pago")
                {
                    Metodo = "ObtenerDetalleVentaPorFornaPago",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleVentaPorFormaPago;
        }

        public async Task<ServiceTypeDetail> ObtenerDetalleVentaPorTipoServicio(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            ServiceTypeDetail detalleVentaPorTipoServicio = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 5);
                    detalleVentaPorTipoServicio = new ServiceTypeDetail();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleVentaPorTipoServicio.chartData = new List<ServiceChartDataItem>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleVentaPorTipoServicio.chartData.Add(new ServiceChartDataItem
                            {
                                name = Convert.ToString(reader["ServiceType"]),
                                value = Convert.ToDouble(reader["TotalSales"])
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleVentaPorTipoServicio.tableData = new List<ServiceTableDataItem>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleVentaPorTipoServicio.tableData.Add(new ServiceTableDataItem
                                {
                                    serviceType = reader["serviceType"].ToString(),
                                    sales = Convert.ToDouble(reader["sales"]),
                                    percentage = Convert.ToDouble(reader["porcentaje"]),
                                    orders = Convert.ToInt32(reader["ordenes"]),
                                    averageTicket = Convert.ToDouble(reader["averageTicket"]),
                                    trend = Convert.ToDouble(reader["trend"]),
                                    trendPositive = Convert.ToBoolean(reader["trendPositive"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle de Venta por Tipo de Servicio")
                {
                    Metodo = "ObtenerDetalleVentaPorTipoServicio",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleVentaPorTipoServicio;
        }

        public async Task<KitchenStationDetail> ObtenerDetalleVentaPorEstacionCocina(int n_Sucursal, int n_FechaInicial, int n_FechaFinal)
        {
            KitchenStationDetail detalleVentaPorEstacionCocina = null;
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
                    cmd.Parameters.AddWithValue("@nTipoDetalle", 6);
                    detalleVentaPorEstacionCocina = new KitchenStationDetail();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primera tabla - ChartData

                        detalleVentaPorEstacionCocina.chartData = new List<KitchenChartDataItem>();
                        //detalleVentasPorCategoria.chartData = [];

                        while (await reader.ReadAsync())
                        {
                            detalleVentaPorEstacionCocina.chartData.Add(new KitchenChartDataItem
                            {
                                name = Convert.ToString(reader["StationName"]),
                                value = Convert.ToDouble(reader["TotalSales"])
                            });
                        }

                        // Segunda tabla - TableData
                        if (await reader.NextResultAsync())
                        {
                            detalleVentaPorEstacionCocina.tableData = new List<KitchenTableDataItem>();
                            //detalleVentasPorCategoria.tableData = [];

                            while (await reader.ReadAsync())
                            {
                                detalleVentaPorEstacionCocina.tableData.Add(new KitchenTableDataItem
                                {
                                    station = reader["station"].ToString(),
                                    sales = Convert.ToDouble(reader["sales"]),
                                    percentage = Convert.ToDouble(reader["porcentaje"]),
                                    products = Convert.ToInt32(reader["productos"]),
                                    averageTicket = Convert.ToDouble(reader["averageTicket"]),
                                    trend = Convert.ToDouble(reader["trend"]),
                                    trendPositive = Convert.ToBoolean(reader["trendPositive"])
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener detalle de Venta por Estación de Cocina")
                {
                    Metodo = "ObtenerDetalleVentaPorEstacionCocina",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalleVentaPorEstacionCocina;
        }
    }
}
