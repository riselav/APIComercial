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
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.Data.Implementaciones
{
    public class RegMovimientoVentaRepositorio : IRegMovimientoVentaRepositorio

    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegMovimientoVentaRepositorio> _logger;

        public RegMovimientoVentaRepositorio(ILogger<RegMovimientoVentaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<RegMovimientoVenta> IME_REG_VentasEncabezado(RegMovimientoVenta regMovimientoVenta)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_IME_REG_VentasEncabezado",
                        CommandType = CommandType.StoredProcedure,
                    };
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


                    if (valorOutput > 0 && regMovimientoVenta.Detalle != null && regMovimientoVenta.Detalle.Count > 0)
                    {
                        foreach (RegMovimientoVentaDetalle detalle in regMovimientoVenta.Detalle)
                        {
                            SqlCommand cmdInsDetalle = new SqlCommand()
                            {
                                Connection = con,
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
                            cmdInsDetalle.Parameters.AddWithValue("@nIDImpuestoIVA", detalle.nIDImpuestoIVA == 0 ? null : detalle.nIDImpuestoIVA);
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
                }
            }
            catch (Exception ex)
            {
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

            return regMovimientoVenta;
        }

    }
}
