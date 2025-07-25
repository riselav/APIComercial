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
                        cmd.Parameters.AddWithValue("@nTipoVenta", regMovimientoVenta.nTipoVenta == 0 ? null : regMovimientoVenta.nTipoVenta);
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

                            await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regMovimientoVenta.regMovimientoCaja, con, transaction);
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
    }
}
