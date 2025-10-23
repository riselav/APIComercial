using Fac_Timbrado_40;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Consultas;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;


namespace Voalaft.Data.Implementaciones
{
    public class RegMovimientoVentaRepositorio : IRegMovimientoVentaRepositorio

    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegMovimientoVentaRepositorio> _logger;
        private readonly IRegMovimientoCajaRepositorio _movimientoCajaRepositorio;
        private readonly IRegAperturaCajaRepositorio _regAperturaCajaRepositorio;

        public RegMovimientoVentaRepositorio(ILogger<RegMovimientoVentaRepositorio> logger, Conexion conexion, 
            IRegMovimientoCajaRepositorio movimientoCajaRepositorio, IRegAperturaCajaRepositorio regAperturaCajaRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _movimientoCajaRepositorio = movimientoCajaRepositorio;
            _regAperturaCajaRepositorio = regAperturaCajaRepositorio;
        }

        public async Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta)
        {
            using (var con = _conexion.ObtenerSqlConexion())
            {
                await con.OpenAsync();

                using (var transaction = con.BeginTransaction())
                {
                    RegAperturaCaja paramApertura = new RegAperturaCaja
                    {
                        IDCaja = (int)regMovimientoVenta.nCaja,
                        IDSucursal = regMovimientoVenta.nSucursal
                    };
                    RegAperturaCaja apertura = await _regAperturaCajaRepositorio.ObtenAperturaAbierta(paramApertura);
                    try
                    {                        
                        SqlCommand cmd = new SqlCommand()
                        {
                            Connection = con,
                            Transaction=transaction,
                            CommandText = "RST_IME_REG_VentasEncabezado",
                            CommandType = CommandType.StoredProcedure,
                        };
                        regMovimientoVenta.nIDApertura = apertura == null ? null : apertura.IDApertura;
                        cmd.Parameters.AddWithValue("@nTipoRegistro", regMovimientoVenta.nTipoRegistro);
                        cmd.Parameters.AddWithValue("@nTipoVenta", regMovimientoVenta.nTipoVenta == 0 ? 2 : regMovimientoVenta.nTipoVenta);
                        cmd.Parameters.AddWithValue("@nSucursal", regMovimientoVenta.nSucursal);
                        cmd.Parameters.AddWithValue("@nCaja", regMovimientoVenta.nCaja);
                        cmd.Parameters.AddWithValue("@nCliente", regMovimientoVenta.nCliente);
                        cmd.Parameters.AddWithValue("@nIdLista", regMovimientoVenta.nIdLista == 0 ? null : regMovimientoVenta.nIdLista);
                        cmd.Parameters.AddWithValue("@nIDApertura", regMovimientoVenta.nIDApertura == 0 ? null : regMovimientoVenta.nIDApertura);
                        cmd.Parameters.AddWithValue("@nFecha", regMovimientoVenta.nFecha);

                        cmd.Parameters.AddWithValue("@nCotizacion", regMovimientoVenta.nCotizacion == 0 ? null : regMovimientoVenta.nCotizacion);
                        cmd.Parameters.AddWithValue("@nVentaOrigenDevolucion", regMovimientoVenta.nVentaOrigenDevolucion == 0 ? null : regMovimientoVenta.nVentaOrigenDevolucion);
                        cmd.Parameters.AddWithValue("@nEmpleado_Registra", regMovimientoVenta.nEmpleadoRegistra);

                        cmd.Parameters.AddWithValue("@nSubtotal", regMovimientoVenta.nSubtotal);
                        cmd.Parameters.AddWithValue("@nImpuestoIVA", regMovimientoVenta.nImpuestoIVA);
                        cmd.Parameters.AddWithValue("@nImpuestoIEPS", regMovimientoVenta.nImpuestoIEPS);
                        cmd.Parameters.AddWithValue("@nImporteDescuento", regMovimientoVenta.nImporteDescuento);
                        cmd.Parameters.AddWithValue("@nPorcentajeDescuento", regMovimientoVenta.nPorcentajeDescuento == 0 ? null : regMovimientoVenta.nPorcentajeDescuento);
                        cmd.Parameters.AddWithValue("@nTotal", regMovimientoVenta.nTotal);

                        cmd.Parameters.AddWithValue("@nIDRegistroCaja", regMovimientoVenta.nIDRegistroCaja == 0 ? null : regMovimientoVenta.nIDRegistroCaja);
                        cmd.Parameters.AddWithValue("@nPagaCon", regMovimientoVenta.nPagaCon);
                        cmd.Parameters.AddWithValue("@nCambio", regMovimientoVenta.nCambio);

                        cmd.Parameters.AddWithValue("@bFactura", regMovimientoVenta.bFactura);
                        cmd.Parameters.AddWithValue("@nImporteFactura", regMovimientoVenta.nImporteFactura);
                        cmd.Parameters.AddWithValue("@nFacturaFinDia", regMovimientoVenta.nFacturaFinDia == 0 ? null : regMovimientoVenta.nFacturaFinDia);
                        cmd.Parameters.AddWithValue("@nFactura", regMovimientoVenta.nFactura == 0 ? null : regMovimientoVenta.nFactura);

                        cmd.Parameters.AddWithValue("@cComentarios", regMovimientoVenta.cComentarios);
                        cmd.Parameters.AddWithValue("@cNombreCliente", regMovimientoVenta.cNombreCliente);
                        cmd.Parameters.AddWithValue("@nEmpleadoCancela", regMovimientoVenta.nEmpleadoCancela == 0 ? null : regMovimientoVenta.nEmpleadoCancela);
                        cmd.Parameters.AddWithValue("@nEmpleadoAutorizaCancelacion", regMovimientoVenta.nEmpleadoAutorizaCancelacion == 0 ? null : regMovimientoVenta.nEmpleadoAutorizaCancelacion);
                        cmd.Parameters.AddWithValue("@nMotivoCancelacion", regMovimientoVenta.nMotivoCancelacion == 0 ? null : regMovimientoVenta.nMotivoCancelacion);
                        cmd.Parameters.AddWithValue("@cObservacionesCancelacion", regMovimientoVenta.cObservacionesCancelacion);

                        cmd.Parameters.AddWithValue("@cUsuario_Registra", regMovimientoVenta.Usuario);
                        cmd.Parameters.AddWithValue("@cMaquina_Registra", regMovimientoVenta.Maquina);
                        cmd.Parameters.AddWithValue("@dFecha", regMovimientoVenta.dFecha);
                        

                        //cmd.Parameters.AddWithValue("@nVenta", regMovimientoVenta.nVenta);
                        SqlParameter outputParam = new SqlParameter("@nVenta", SqlDbType.BigInt);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                        //catMarca.Marca = folioSig;

                        long valorOutput = (long)cmd.Parameters["@nVenta"].Value;

                        regMovimientoVenta.nVenta = valorOutput;
                        if (valorOutput > 0 && regMovimientoVenta.Detalle != null && regMovimientoVenta.Detalle.Count > 0)
                        {
                            foreach (RegMovimientoVentaDetalle detalle in regMovimientoVenta.Detalle)
                            {
                                detalle.nVenta = valorOutput;
                                SqlCommand cmdInsDetalle = new SqlCommand()
                                {
                                    Connection = con,
                                    Transaction = transaction,
                                    CommandText = "RST_IME_REG_VentasDetalle",
                                    CommandType = CommandType.StoredProcedure,
                                };
                                cmdInsDetalle.Parameters.Add("@nVenta", SqlDbType.BigInt).Value = valorOutput;
                                cmdInsDetalle.Parameters.AddWithValue("@nIDArticulo", detalle.nIDArticulo);
                                cmdInsDetalle.Parameters.AddWithValue("@nCantidad", detalle.nCantidad);

                                cmdInsDetalle.Parameters.AddWithValue("@nCantidadDevuelta", detalle.nCantidadDevuelta);
                                cmdInsDetalle.Parameters.AddWithValue("@nPrecioUnitario", detalle.nPrecioUnitario);
                                cmdInsDetalle.Parameters.AddWithValue("@nPrecioOriginal", detalle.nPrecioOriginal);
                                cmdInsDetalle.Parameters.AddWithValue("@nSubtotal", detalle.nSubtotal);
                                cmdInsDetalle.Parameters.AddWithValue("@nImpuestoIVA", detalle.nImpuestoIVA);
                                cmdInsDetalle.Parameters.AddWithValue("@nIDImpuestoIVA", detalle.nIDImpuestoIVA);
                                cmdInsDetalle.Parameters.AddWithValue("@nPorcentajeImpuestoIVA", detalle.nPorcentajeImpuestoIVA);

                                cmdInsDetalle.Parameters.AddWithValue("@nImpuestoIEPS", detalle.nImpuestoIEPS);
                                cmdInsDetalle.Parameters.AddWithValue("@nIDImpuestoIEPS", detalle.nIDImpuestoIEPS == 0 ? null : detalle.nIDImpuestoIEPS);
                                cmdInsDetalle.Parameters.AddWithValue("@nPorcentajeImpuestoIEPS", detalle.nPorcentajeImpuestoIEPS);
                                cmdInsDetalle.Parameters.AddWithValue("@nImporteDescuento", detalle.nImporteDescuento);
                                cmdInsDetalle.Parameters.AddWithValue("@nPorcentajeDescuento", detalle.nPorcentajeDescuento);
                                cmdInsDetalle.Parameters.AddWithValue("@nTotal", detalle.nTotal);
                                cmdInsDetalle.Parameters.AddWithValue("@cComentarios", detalle.cComentarios);
                                cmdInsDetalle.Parameters.AddWithValue("@nCostoUnitario", detalle.nCostoUnitario);

                                cmdInsDetalle.Parameters.AddWithValue("@cUsuario_Registra", regMovimientoVenta.Usuario);
                                cmdInsDetalle.Parameters.AddWithValue("@cMaquina_Registra", regMovimientoVenta.Maquina);

                                SqlParameter returnValue = cmdInsDetalle.Parameters.Add("@ReturnVal", SqlDbType.Int);
                                returnValue.Direction = ParameterDirection.ReturnValue;

                                await cmdInsDetalle.ExecuteNonQueryAsync();
                                int folio = (int)returnValue.Value;
                            }
                        }

                        if (regMovimientoVenta.nIDApertura > 0 && regMovimientoVenta.regMovimientoCaja != null)
                        {
                            long nIDApertura = 0;

                            if (regMovimientoVenta.nIDApertura != null) 
                                nIDApertura = (long)regMovimientoVenta.nIDApertura;

                            regMovimientoVenta.regMovimientoCaja.IDApertura = nIDApertura;
                            regMovimientoVenta.regMovimientoCaja.Usuario= regMovimientoVenta.Usuario;
                            regMovimientoVenta.regMovimientoCaja.Maquina = regMovimientoVenta.Maquina;

                            var mc = await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regMovimientoVenta.regMovimientoCaja, con, transaction);
                            regMovimientoVenta.nIDRegistroCaja = mc.IDRegistroCaja;
                            if(regMovimientoVenta.nIDRegistroCaja != null && regMovimientoVenta.nIDRegistroCaja > 0 )
                            {
                                SqlCommand cmdUpd = new SqlCommand()
                                {
                                    Connection = con,
                                    Transaction = transaction,
                                    CommandText = "UPDATE VTA_MovimientosVenta SET nIDRegistroCaja = @nIDRegistroCaja WHERE nVenta = @nVenta",
                                    CommandType = CommandType.Text,
                                };

                                cmdUpd.Parameters.AddWithValue("@nIDRegistroCaja", regMovimientoVenta.nIDRegistroCaja);
                                cmdUpd.Parameters.AddWithValue("@nVenta", regMovimientoVenta.nVenta);

                                await cmdUpd.ExecuteNonQueryAsync();
                                
                            }
                            //await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regMovimientoCaja); // sin pasar conexión ni transacción

                        }

                        // Todo bien, commit
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                        string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                        int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                        _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                        throw new DataAccessException("Error(rp) al insertar cabecero de venta")
                        {
                            Metodo = "Lista",
                            ErrorMessage = ex.Message,
                            ErrorCode = 1
                        };
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return regMovimientoVenta;
        }

        public async Task<List<MovimientoVentaEnc>> CM_CON_Todas_Cotizaciones(int nSucursal)
        {
            List<MovimientoVentaEnc> cotizaciones = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_CON_Todas_Cotizaciones",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", nSucursal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            cotizaciones.Add(
                                new MovimientoVentaEnc()
                                {
                                    nVenta = ConvertUtils.ToInt64(reader["nVenta"]),
                                    nTipoRegistro = ConvertUtils.ToInt32(reader["nTipoRegistro"]),
                                    nTipoVenta = ConvertUtils.ToInt32(reader["nTipoVenta"]),
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nCaja = ConvertUtils.ToInt32(reader["nCaja"]),                                    
                                    nCliente = ConvertUtils.ToInt32(reader["nCliente"]),
                                    cNombreCompleto = ConvertUtils.ToString(reader["cNombreCompleto"]),
                                    nIDApertura = ConvertUtils.ToInt64(reader["nIDApertura"]),
                                    nConsecutivo = ConvertUtils.ToInt64(reader["nConsecutivo"]),
                                    nSubtotal = ConvertUtils.ToDecimal(reader["nSubtotal"]),
                                    nImpuestoIVA = ConvertUtils.ToDecimal(reader["nImpuestoIVA"]),
                                    nImpuestoIEPS = ConvertUtils.ToDecimal(reader["nImpuestoIEPS"]),
                                    nImporteDescuento = ConvertUtils.ToDecimal(reader["nImporteDescuento"]),
                                    nTotal = ConvertUtils.ToDecimal(reader["nTotal"]),
                                    cComentarios = ConvertUtils.ToString(reader["cComentarios"])
                                }
                                );
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
                throw new DataAccessException("Error(rp) No se pudo obtener las cotizaciones")
                {
                    Metodo = "CM_CON_Todas_Cotizaciones",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cotizaciones;
        }

        public async Task<List<MovimientoVentaDet>> CM_CON_detalle_mov_ventas(long nVenta)
        {
            List<MovimientoVentaDet> detalle = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_CON_detalle_mov_ventas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nVenta", nVenta);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            detalle.Add(
                                new MovimientoVentaDet()
                                {
                                    nVenta = ConvertUtils.ToInt64(reader["nVenta"]),
                                    nRenglon = ConvertUtils.ToInt32(reader["nRenglon"]),
                                    nIDArticulo = ConvertUtils.ToInt32(reader["nIDArticulo"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nCantidad = ConvertUtils.ToDecimal(reader["nCantidad"]),
                                    nCantidadDevuelta = ConvertUtils.ToDecimal(reader["nCantidadDevuelta"]),
                                    nPrecioUnitario = ConvertUtils.ToDecimal(reader["nPrecioUnitario"]),
                                    nPrecioOriginal = ConvertUtils.ToDecimal(reader["nPrecioOriginal"]),
                                    nSubtotal = ConvertUtils.ToDecimal(reader["nSubtotal"]),
                                    nImpuestoIVA = ConvertUtils.ToDecimal(reader["nImpuestoIVA"]),
                                    nIDImpuestoIVA = ConvertUtils.ToInt32(reader["nIDImpuestoIVA"]),
                                    nPorcentajeImpuestoIVA = ConvertUtils.ToDecimal(reader["nPorcentajeImpuestoIVA"]),
                                    nImpuestoIEPS = ConvertUtils.ToDecimal(reader["nImpuestoIEPS"]),
                                    nIDImpuestoIEPS = ConvertUtils.ToInt32(reader["nIDImpuestoIEPS"]),
                                    nPorcentajeImpuestoIEPS = ConvertUtils.ToDecimal(reader["nPorcentajeImpuestoIEPS"]),
                                    nImporteDescuento = ConvertUtils.ToDecimal(reader["nImporteDescuento"]),
                                    nPorcentajeDescuento = ConvertUtils.ToDecimal(reader["nPorcentajeDescuento"]),
                                    nTotal = ConvertUtils.ToDecimal(reader["nTotal"]),
                                    cComentarios = ConvertUtils.ToString(reader["cComentarios"])
                                }
                                );
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
                throw new DataAccessException("Error(rp) No se pudo obtener las ventas detalle")
                {
                    Metodo = "CM_CON_detalle_mov_ventas",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalle;
        }

        public async Task<ImpresionTicketData> Obtener_Ticket_Venta(long nVenta)
        {
            const int LINE_WIDTH = 40;

            var lineas = new List<string>();
            DateTime fechaActual = DateTime.Now; // Fecha y hora actual en Culiacán, Sinaloa

            // --- Encabezado del Ticket ---
            lineas.Add("".PadLeft(LINE_WIDTH, '-'));
            lineas.Add("SUPER TIENDA EL SOL".PadLeft(LINE_WIDTH / 2 + "SUPER TIENDA EL SOL".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("SUCURSAL ZAPATA".PadLeft(LINE_WIDTH / 2 + "SUCURSAL ZAPATA".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("AV. JUAREZ #567 OTE.".PadLeft(LINE_WIDTH / 2 + "AV. JUAREZ #567 OTE.".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("CULIACAN, SINALOA".PadLeft(LINE_WIDTH / 2 + "CULIACAN, SINALOA".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("RFC: ABC123456XYZ".PadRight(LINE_WIDTH, ' '));
            lineas.Add($"Venta #:{nVenta.ToString().PadRight(LINE_WIDTH - "Venta #:".Length, ' ')}");
            lineas.Add($"Fecha: {fechaActual.ToString("dd/MM/yyyy HH:mm").PadRight(LINE_WIDTH - "Fecha: ".Length, ' ')}");
            lineas.Add("Cajero: ANA LOPEZ".PadRight(LINE_WIDTH, ' '));
            lineas.Add("".PadLeft(LINE_WIDTH, '-'));
            lineas.Add("DESCRIPCION      CANT.   PRECIO   IMPORTE");
            lineas.Add("".PadLeft(LINE_WIDTH, '-'));

            // --- Artículos de Venta (simulados) ---
            var items = new List<dynamic>
        {
            new { Nombre = "AGUA EMBOTELLADA 1L", Cantidad = 3, Precio = 12.50m },
            new { Nombre = "REFRESCO COLA 600ML", Cantidad = 2, Precio = 18.00m },
            new { Nombre = "PAPAS FRITAS GRANDES", Cantidad = 1, Precio = 30.00m },
            new { Nombre = "CHOCOLATE BARRA 100G", Cantidad = 4, Precio = 15.00m },
            new { Nombre = "CHICLE MENTA 5PZ", Cantidad = 5, Precio = 5.00m }
        };

            decimal subtotal = 0m;
            foreach (var item in items)
            {
                decimal importe = item.Cantidad * item.Precio;
                subtotal += importe;

                // Ajusta los anchos para que sumen LINE_WIDTH
                string namePart = item.Nombre.Length > 15 ? item.Nombre.Substring(0, 15).PadRight(15, ' ') : item.Nombre.PadRight(15, ' ');
                string qtyPart = item.Cantidad.ToString().PadLeft(5, ' ');
                string pricePart = item.Precio.ToString("F2").PadLeft(8, ' ');
                string importPart = importe.ToString("F2").PadLeft(9, ' '); // Total de cada linea

                lineas.Add($"{namePart} {qtyPart} {pricePart} {importPart}");
            }

            lineas.Add("".PadLeft(LINE_WIDTH, '-'));

            // --- Resumen de Totales ---
            decimal ivaTasa = 0.16m; // IVA del 16% en México
            decimal iva = subtotal * ivaTasa;
            decimal descuentoSimulado = subtotal * 0.03m; // 3% de descuento simulado
            decimal totalGeneral = subtotal + iva - descuentoSimulado;

            lineas.Add($"SUBTOTAL:{subtotal.ToString("F2").PadLeft(LINE_WIDTH - "SUBTOTAL:".Length, ' ')}");
            lineas.Add($"IVA (16%):{iva.ToString("F2").PadLeft(LINE_WIDTH - "IVA (16%):".Length, ' ')}");
            lineas.Add($"DESCUENTO:{descuentoSimulado.ToString("F2").PadLeft(LINE_WIDTH - "DESCUENTO:".Length, ' ')}");
            lineas.Add("".PadLeft(LINE_WIDTH, '='));
            lineas.Add($"TOTAL:{totalGeneral.ToString("F2").PadLeft(LINE_WIDTH - "TOTAL:".Length, ' ')}");
            lineas.Add("".PadLeft(LINE_WIDTH, '='));

            // --- Pie de Ticket ---
            lineas.Add(""); // Línea en blanco
            lineas.Add("EFECTIVO:".PadRight(LINE_WIDTH - "$1000.00".Length, ' ') + "$1000.00"); // Simula pago con un billete
            lineas.Add("CAMBIO:".PadRight(LINE_WIDTH - totalGeneral.ToString("F2").Length, ' ') + (1000m - totalGeneral).ToString("F2")); // Calcula el cambio
            lineas.Add("");
            lineas.Add("¡GRACIAS POR SU PREFERENCIA!".PadLeft(LINE_WIDTH / 2 + "¡GRACIAS POR SU PREFERENCIA!".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("Visítanos en www.nuestrocomercio.mx".PadLeft(LINE_WIDTH / 2 + "Visítanos en www.nuestrocomercio.mx".Length / 2, ' ').PadRight(LINE_WIDTH, ' '));
            lineas.Add("".PadLeft(LINE_WIDTH, '-'));
            lineas.Add("\n\n\n\n"); // Múltiples saltos de línea para el corte del papel

            return new ImpresionTicketData
            {
                nVenta = nVenta,
                LineasImprimibles = lineas
            };
        }

        public enum TipoRegistro
        {
            Venta = 1,
            Cotizacion = 3
        }

        public async Task<List<ReporteVentas>> CM_CON_reporte_ventas_sp(ParametrosReporteVentas parametrosReporteVentas)
        {
            var ventasMap = new Dictionary<long, ReporteVentas>();
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_CON_reporte_ventas_sp",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCajero", parametrosReporteVentas.nCajero == 0 ? null : parametrosReporteVentas.nCajero);
                    cmd.Parameters.AddWithValue("@bEstatus", parametrosReporteVentas.bEstatus);
                    cmd.Parameters.AddWithValue("@nTipoVenta", parametrosReporteVentas.nTipoVenta == 0 ? null : parametrosReporteVentas.nTipoVenta);
                    cmd.Parameters.AddWithValue("@fechaInicio", parametrosReporteVentas.fechaInicio);
                    cmd.Parameters.AddWithValue("@fechaFin", parametrosReporteVentas.fechaFin);
                    cmd.Parameters.AddWithValue("@cBusquedaGeneral", parametrosReporteVentas.cBusquedaGeneral == "" ? null : parametrosReporteVentas.cBusquedaGeneral);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            var folio = ConvertUtils.ToInt64(reader["folio"]);
                            var nVenta = ConvertUtils.ToInt64(reader["nVenta"]);

                            // Si el folio no existe en el diccionario, crea una nueva venta
                            if (!ventasMap.TryGetValue(nVenta, out var venta))
                            {
                                TipoRegistro cTipoRegistro = (TipoRegistro)ConvertUtils.ToInt32(reader["nTipoRegistro"]);
                                venta = new ReporteVentas
                                {
                                    folio = folio,
                                    nVenta = nVenta,
                                    nTipoRegistro = ConvertUtils.ToInt32(reader["nTipoRegistro"]),
                                    cTipoRegistro = cTipoRegistro.ToString(),
                                    fechaHora = ConvertUtils.ToDateTime(reader["fechaHora"]),
                                    nEmpleadoRegistra = ConvertUtils.ToInt32(reader["nEmpleado_Registra"]),
                                    cajero = ConvertUtils.ToString(reader["cajero"]),
                                    nCliente = reader["nCliente"] is DBNull ? (int?)null : ConvertUtils.ToInt32(reader["nCliente"]),
                                    cNombreCompleto = ConvertUtils.ToString(reader["cNombreCompleto"]),
                                    importe = ConvertUtils.ToDecimal(reader["importe"]),
                                    nFactura = ConvertUtils.ToInt64(reader["nFactura"]), // Se mapea como string o int?
                                    cComentarios = ConvertUtils.ToString(reader["cComentarios"]),
                                    bActivo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    listReporteVentasDetalle = new List<ReporteVentasDetalle>()
                                };
                                ventasMap.Add(nVenta, venta);
                            }

                            // Agrega el detalle a la venta correspondiente
                            venta.listReporteVentasDetalle.Add(
                                new ReporteVentasDetalle
                                {
                                    nIDArticulo = ConvertUtils.ToInt32(reader["nIDArticulo"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nCantidad = ConvertUtils.ToDecimal(reader["nCantidad"]),
                                    nPrecioUnitario = ConvertUtils.ToDecimal(reader["nPrecioUnitario"]),
                                    nTotal = ConvertUtils.ToDecimal(reader["nTotal"])
                                }
                            );
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
                throw new DataAccessException("Error(rp) No se pudo obtener las ventas")
                {
                    Metodo = "CM_CON_reporte_ventas_sp",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return ventasMap.Values.ToList();
        }

        public async Task<ParamCancelaVenta> IME_CAN_Cancelar_Venta(ParamCancelaVenta paramCancelaVenta)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "IME_CAN_Cancelar_Venta",
                        CommandType = CommandType.StoredProcedure,
                    };

                    cmd.Parameters.AddWithValue("@nVenta", paramCancelaVenta.nVenta);
                    cmd.Parameters.AddWithValue("@nEmpleadoCancela", paramCancelaVenta.usuarioCancelo);
                    cmd.Parameters.AddWithValue("@nEmpleadoAutorizaCancelacion", paramCancelaVenta.usuarioAutorizo);
                    cmd.Parameters.AddWithValue("@nMotivoCancelacion", paramCancelaVenta.motivo);
                    cmd.Parameters.AddWithValue("@cObservacionesCancelacion", paramCancelaVenta.observacion ?? (object)DBNull.Value);

                    // Mapeo de los nombres de usuario y máquina
                    cmd.Parameters.AddWithValue("@cUsuario_Cancela", paramCancelaVenta.Usuario ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@cMaquina_Cancela", paramCancelaVenta.Maquina ?? (object)DBNull.Value);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar la venta con nVenta: {nVenta}", paramCancelaVenta.nVenta);
                throw new DataAccessException("Error(rp) No se pudo cancelar la venta.")
                {
                    Metodo = "CancelarVenta",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return paramCancelaVenta;
        }

        public async Task<List<FormasPagoImporte>> CM_CON_FormasPago_Venta(long nVenta)
        {
            List<FormasPagoImporte> detalle = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_CON_FormasPago_Venta",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nVenta", nVenta);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            detalle.Add(
                                new FormasPagoImporte()
                                {
                                    FormaPago = ConvertUtils.ToInt32(reader["nFormaPago"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Importe = ConvertUtils.ToDecimal(reader["nImporte"])                                    
                                }
                                );
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
                throw new DataAccessException("Error(rp) No se pudo obtener las formas de pago")
                {
                    Metodo = "CM_CON_FormasPago_Venta",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return detalle;
        }

        public async Task<List<ImpresionData>> TicketVenta(int nSucursal, long nVenta)
        {
            List<ImpresionData> impresion = [];
            
            try
            {
                //TimbraFactura();
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "COM_TicketVenta",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", nSucursal);
                    cmd.Parameters.AddWithValue("@nFolio", nVenta);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            impresion.Add(
                                new ImpresionData()
                                {
                                    nRenglon = ConvertUtils.ToInt32(reader["nRenglon"]),
                                    nRenglonConcepto = ConvertUtils.ToInt32(reader["nRenglonConcepto"]),
                                    nRenglonMod = ConvertUtils.ToInt32(reader["nRenglonMod"]),
                                    cLinea = ConvertUtils.ToBoolean(reader["bCodBarra"]) ? BarcodeGenerator.GenerateBarcodeBase64(ConvertUtils.ToString(reader["cLinea"])) : ConvertUtils.ToString(reader["cLinea"]),
                                    bLetraGrande = ConvertUtils.ToBoolean(reader["bLetraGrande"]),
                                    bNegrita = ConvertUtils.ToBoolean(reader["bNegrita"]),
                                    bCodBarra = ConvertUtils.ToBoolean(reader["bCodBarra"])
                                }
                                );
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
                throw new DataAccessException("Error(rp) No se pudo obtener el ticket")
        {
            Metodo = "TicketCorteCaja",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
    }

            return impresion;
        }

        private async void TimbraFactura(SqlConnection externalConnection = null,
                SqlTransaction externalTransaction = null)
        {
            try
            {
                bool shouldCloseConnection = false;
                bool shouldCommitTransaction = false;

                SqlConnection con = externalConnection;
                SqlTransaction transaction = externalTransaction;
                //SqlConnection con = externalConnection;
                //SqlTransaction transaction = externalTransaction;

                if (con == null)
                {
                    con = _conexion.ObtenerSqlConexion();
                    await con.OpenAsync();
                    shouldCloseConnection = true;
                }

                if (transaction == null)
                {
                    transaction = con.BeginTransaction();
                    shouldCommitTransaction = true;
                }
                //System.Data.SqlClient.SqlConnection conSql = new System.Data.SqlClient.SqlConnection();
                var conn = new SQLConnector.SQLConnector.SQLCONN("","","","");
                var conecio = conn.InitConexionExterna(con , transaction);

                var vDLlTimbra = new Fac_Timbrado_40.CLS_CFDI();
                var vOBJ_com = new Fac_Timbrado_40.CLS_COM();
                var vOBJComprobante = new Fac_Timbrado_40.CLS_Comprobante();
                Fac_Timbrado_40.clsEmisores objemisor = Fac_Timbrado_40.clsLeerCatalogosBD.ObtenEmisor(2, ref conecio);
                Fac_Timbrado_40.clsConfiguracionEmisor objconfEmisor = Fac_Timbrado_40.clsLeerCatalogosBD.ObtenConfiguracionEmisor(objemisor.Folio, ref conecio);
                //Fac_Timbrado_40.clsEmisores objemisor = Fac_Timbrado_40.clsEmisores.Obten(3);
                //Fac_Timbrado_40.clsEmisores objemisor = new clsEmisores(2);
                //The type initializer for 'Fac_Timbrado_40.clsLeerCatalogosBD' threw an exception.
                //Fac_Timbrado_40.clsConfiguracionEmisor objconfEmisor = Fac_Timbrado_40.clsConfiguracionEmisor.Obten(objemisor.Folio);
                //Fac_Timbrado_40.clsConfiguracionEmisor objconfEmisor = new clsConfiguracionEmisor(2);
                objconfEmisor.NumeroCertificado = "00001000000518313647";

                vOBJComprobante.Version = "4.0";
                vOBJComprobante.Sucursal = 1;
                vOBJComprobante.Serie = "A";
                vOBJComprobante.Folio = "40";
                vOBJComprobante.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                vOBJComprobante.FormaPago = "99";
                vOBJComprobante.NoCertificado = objconfEmisor.NumeroCertificado;
                vOBJComprobante.CondicionesDePago = "NA";
                vOBJComprobante.SubTotal = "1.0";
                vOBJComprobante.Moneda = "MXN";
                vOBJComprobante.TipoCambio = "1";
                vOBJComprobante.Total = "1.16";
                vOBJComprobante.TipoDeComprobante = "I";
                vOBJComprobante.Exportacion = "01";
                vOBJComprobante.MetodoPago = "PPD";
                vOBJComprobante.LugarExpedicion = objemisor.CodigoPostal;

                vOBJComprobante.Emisor = new Fac_Timbrado_40.clsEmisores();
                vOBJComprobante.Emisor.Folio = objemisor.Folio;
                vOBJComprobante.Emisor.RFC = objemisor.RFC;
                vOBJComprobante.Emisor.RazonSocial = objemisor.RazonSocial;
                vOBJComprobante.Emisor.RegimenFiscal = objemisor.RegimenFiscal;

                vOBJComprobante.Receptor = new Fac_Timbrado_40.CLS_Receptor();
                vOBJComprobante.Receptor.Rfc = "GACA781221D27";
                vOBJComprobante.Receptor.Nombre = "ANGEL GARCIA CISNEROS";
                vOBJComprobante.Receptor.DomicilioFiscalReceptor = "80019";
                vOBJComprobante.Receptor.RegimenFiscalReceptor = "612";
                vOBJComprobante.Receptor.UsoCFDI = "G03";

                vOBJComprobante.Cliente = 2;
                vOBJComprobante.DomilicioReceptor = vOBJComprobante.Receptor.DomicilioCliente;
                vOBJComprobante.LocalidadReceptor = vOBJComprobante.Receptor.LocalidadCliente;

                var vConveptos = new Fac_Timbrado_40.CLS_Conceptos[1];
                vConveptos[0] = new Fac_Timbrado_40.CLS_Conceptos();

                vConveptos[0].ClaveProdServ = "01010101";
                vConveptos[0].NoIdentificacion = "miclave";
                vConveptos[0].Cantidad = "1";
                vConveptos[0].ClaveUnidad = "F52";
                vConveptos[0].Unidad = "TONELADA";
                vConveptos[0].Descripcion = "ACERO";
                vConveptos[0].ValorUnitario = "1.00";
                vConveptos[0].Importe = "1.00";
                vConveptos[0].ObjetoImp = "02";

                var vOBJImpuestos = new Fac_Timbrado_40.CLS_Conceptos_Impuestos();

                var vTraslados = new Fac_Timbrado_40.CLS_Conceptos_Impuestos_Traslados[1];
                vTraslados[0] = new Fac_Timbrado_40.CLS_Conceptos_Impuestos_Traslados();

                vTraslados[0].Base = "1.0";
                vTraslados[0].Impuesto = "002";
                vTraslados[0].TipoFactor = "Tasa";
                vTraslados[0].TasaOCuota = "0.160000";
                vTraslados[0].Importe = "0.16";

                vOBJImpuestos.Traslados = vTraslados;
                vConveptos[0].Impuestos = vOBJImpuestos;
                vOBJComprobante.Conceptos = vConveptos;

                var vImpuestos = new Fac_Timbrado_40.CLS_Impuestos();

                vImpuestos.TotalImpuestosTrasladados = "0.16";

                var vOBJTraslados = new Fac_Timbrado_40.CLS_Traslados[1];
                vOBJTraslados[0] = new Fac_Timbrado_40.CLS_Traslados();

                vOBJTraslados[0].Base = "1.0";
                vOBJTraslados[0].Impuesto = "002";
                vOBJTraslados[0].TipoFactor = "Tasa";
                vOBJTraslados[0].TasaOCuota = "0.160000";
                vOBJTraslados[0].Importe = "0.16";

                vImpuestos.Traslados = vOBJTraslados;
                vOBJComprobante.Impuestos = vImpuestos;
                vOBJComprobante.Sistema = "GEMA";
                vOBJComprobante.Domicilio = "REY MELCHOR 7515";
                vOBJComprobante.Localidad = "LOMA DE RODRIGUERA";
                vOBJComprobante.Municipio = "Culiacan";
                vOBJComprobante.Estado = "SINALOA";
                vOBJComprobante.Pais = "MEXICO";
                vOBJComprobante.Comentarios = "RECIBO 045123";

                vOBJComprobante.Correo = new string[] { "riselav87@gmail.com", "angeliasoftpro@gmail.com", "Juan_Pablo_CA@hotmail.com" };


                vOBJComprobante.LogoBase64 = "";
                //if (picLogo.Image != null)
                //{
                //    vOBJComprobante.LogoBase64 = clsGenerales.ImageToBase64(picLogo.Image, System.Drawing.Imaging.ImageFormat.Png);
                //}

                //var conn = new SQLConnector.SQLConnector.SQLCONN
                

                string folioFactura = "";
                var objRecibo = new clsComprobanteEmitido();
                string prmJSON = "";

                if (!CLS_CFDI.GuardaComprobanteWEB(ref vOBJComprobante, ref conn, ref objRecibo, ref prmJSON, true,true))
                {
                    if (conn.TieneTransaccionAbierta())
                    {
                        conn.DeshaceTransaccion();
                    }
                    // clsGenerales.MuestraMensaje("Error al guardar la factura", MessageBoxIcon.Warning);
                    return;
                }

                if (conn.TieneTransaccionAbierta())
                {
                    conn.CierraTransaccion();
                }
                //clsGenerales.MuestraMensaje("Generación de factura exitosa", MessageBoxIcon.Information);
                if (objRecibo != null)
                {
                }

            }
            catch (Exception ex)
            {                
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                //int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                //_logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al insertar Movimiento de Caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            finally
            {
                
            }

            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Fac_Timbrado_40.CLS_UTILERIAS.Genera_Archivo_Cer_Pem("C:\\Ruta", "C:\\OpenSSL-Win64\\bin\\openssl.exe", "txeNombreCer.Text.Trim()");
            clsGenerales.MuestraMensaje("Archivo .cer.pem generado con éxito", MessageBoxIcon.Information);
        }
    }
}
