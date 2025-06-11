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
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class RegMovimientoCajaRepositorio:IRegMovimientoCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegMovimientoCajaRepositorio> _logger;

        public RegMovimientoCajaRepositorio(ILogger<RegMovimientoCajaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<RegMovimientoCaja> IME_REG_MovimientoCaja(RegMovimientoCaja regMovimientoCaja,
                SqlConnection externalConnection = null,
                SqlTransaction externalTransaction = null)
        {
            bool shouldCloseConnection = false;
            bool shouldCommitTransaction = false;

            SqlConnection con = externalConnection;
            SqlTransaction transaction = externalTransaction;

            try
            {
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
                                
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    Transaction = transaction,
                    CommandText = "CM_IME_CAJ_MovimientoCaja",
                    CommandType = CommandType.StoredProcedure,
                };

                SqlParameter outputParam = new SqlParameter("@nFolio", SqlDbType.BigInt);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                cmd.Parameters.AddWithValue("@nTipoRegistro", regMovimientoCaja.TipoRegistroCaja);
                cmd.Parameters.AddWithValue("@nSucursal", regMovimientoCaja.Sucursal);
                cmd.Parameters.AddWithValue("@nIDApertura", regMovimientoCaja.IDApertura);
                cmd.Parameters.AddWithValue("@dFecha", regMovimientoCaja.Fecha);
                cmd.Parameters.AddWithValue("@nConceptoCaja", regMovimientoCaja.ConceptoCaja == 0 ? null : regMovimientoCaja.ConceptoCaja);

                cmd.Parameters.AddWithValue("@nEmpleado", regMovimientoCaja.EmpleadoInvolucrado);
                cmd.Parameters.AddWithValue("@nImporte", regMovimientoCaja.Importe);

                cmd.Parameters.AddWithValue("@cObservaciones", regMovimientoCaja.Observaciones == "" ? null : regMovimientoCaja.Observaciones);
                cmd.Parameters.AddWithValue("@bActivo", regMovimientoCaja.Activo);
                    
                cmd.Parameters.AddWithValue("@cUsuario", regMovimientoCaja.Usuario);
                cmd.Parameters.AddWithValue("@cNombreMaquina", regMovimientoCaja.Maquina);

                cmd.Parameters.AddWithValue("@cUsuarioAutoriza", regMovimientoCaja.UsuarioAutorizaRegistro);
                cmd.Parameters.AddWithValue("@dFechaRegistro", null);
                cmd.Parameters.AddWithValue("@bRegistroEspecial", 0);

                await cmd.ExecuteNonQueryAsync();

                //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                //catMarca.Marca = folioSig;

                regMovimientoCaja.IDRegistroCaja = (long)cmd.Parameters["@nFolio"].Value;

                if (regMovimientoCaja.IDRegistroCaja > 0 && regMovimientoCaja.Detalle != null && regMovimientoCaja.Detalle.Count > 0)
                {
                    foreach (RegMovimientoCajaDetalle detalle in regMovimientoCaja.Detalle)
                    {
                        detalle.IDRegistroCaja = regMovimientoCaja.IDRegistroCaja;
                        SqlCommand cmdInsDetalle = new SqlCommand()
                        {
                            Connection = con,
                            Transaction = transaction,
                            CommandText = "CM_IME_CAJ_DetalleMovimientosCaja",
                            CommandType = CommandType.StoredProcedure,
                        };
                        cmdInsDetalle.Parameters.Add("@nIDRegistroCaja", SqlDbType.BigInt).Value = regMovimientoCaja.IDRegistroCaja;
                        cmdInsDetalle.Parameters.AddWithValue("@nRenglon", detalle.Renglon);
                        cmdInsDetalle.Parameters.AddWithValue("@nFormaPago", detalle.FormaPago);

                        cmdInsDetalle.Parameters.AddWithValue("@nImporte", detalle.Importe);
                        cmdInsDetalle.Parameters.AddWithValue("@bActivo", detalle.Activo);
                            
                        cmdInsDetalle.Parameters.AddWithValue("@cUsuario_Registra", regMovimientoCaja.Usuario);
                        cmdInsDetalle.Parameters.AddWithValue("@cMaquina_Registra", regMovimientoCaja.Maquina);
                        cmdInsDetalle.Parameters.AddWithValue("@dFecha_Registra", DateTime.Now);

                        cmdInsDetalle.Parameters.AddWithValue("@nOrden", 0);
                        cmdInsDetalle.Parameters.AddWithValue("@nCuenta", 0);
                        cmdInsDetalle.Parameters.AddWithValue("@cReferencia", detalle.Referencia);
                        cmdInsDetalle.Parameters.AddWithValue("@cReferenciaCuponVale", null);

                        cmdInsDetalle.Parameters.AddWithValue("@nImporteFacturado", detalle.ImporteFacturado);
                        cmdInsDetalle.Parameters.AddWithValue("@nImportePropina", detalle.ImportePropina);

                        cmdInsDetalle.Parameters.AddWithValue("@nCliente", detalle.Cliente == 0 ? null : detalle.Cliente);
                        cmdInsDetalle.Parameters.AddWithValue("@nEmpleado", detalle.Empleado == 0 ? null : detalle.Empleado);
                        cmdInsDetalle.Parameters.AddWithValue("@nPropina", detalle.Propina);
                        cmdInsDetalle.Parameters.AddWithValue("@nPagaCon", detalle.PagaCon);
                        cmdInsDetalle.Parameters.AddWithValue("@nCambio", detalle.Cambio);

                        SqlParameter returnValue = cmdInsDetalle.Parameters.Add("@ReturnVal", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;

                        await cmdInsDetalle.ExecuteNonQueryAsync();
                        int folio = (int)returnValue.Value;

                        if (detalle.DetalleDenominacion != null && detalle.DetalleDenominacion.Count > 0)
                        {
                            foreach (RegMovimientoCajaDetalleDenominacion detalleDenominacion in detalle.DetalleDenominacion)
                            {
                                SqlCommand cmdInsDetalleDenominacion = new SqlCommand()
                                {
                                    Connection = con,
                                    Transaction = transaction,
                                    CommandText = "CM_IME_CAJ_DetalleDenominacionMovimientosCaja",
                                    CommandType = CommandType.StoredProcedure,
                                };
                                cmdInsDetalleDenominacion.Parameters.Add("@nFolio", SqlDbType.BigInt).Value = regMovimientoCaja.IDRegistroCaja;
                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@nRenglon", detalle.Renglon);
                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@nDenominacion", detalleDenominacion.Denominacion);

                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@nValor", detalleDenominacion.Valor);
                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@nCantidad", detalleDenominacion.Cantidad);
                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@nImporte", detalleDenominacion.Importe);

                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@cUsuario", regMovimientoCaja.Usuario);
                                cmdInsDetalleDenominacion.Parameters.AddWithValue("@cMaquina", regMovimientoCaja.Maquina);

                                SqlParameter returnValueDen = cmdInsDetalleDenominacion.Parameters.Add("@ReturnVal", SqlDbType.Int);
                                returnValue.Direction = ParameterDirection.ReturnValue;

                                await cmdInsDetalleDenominacion.ExecuteNonQueryAsync();
                                int folioDen = (int)returnValue.Value;

                            }
                        }                                
                    }
                }

                if (shouldCommitTransaction)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                if (shouldCommitTransaction)
                    transaction?.Rollback();

                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al insertar Movimiento de Caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            finally
            {
                if (shouldCloseConnection && con != null)
                    con.Close();
            }

            return regMovimientoCaja;
        }

        public async Task<List<RegMovimientoCaja>> ObtenMovimientosCaja(ParametrosConsultaMovimientosCaja parametros)
        {
            List<RegMovimientoCaja> movimientos = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_CON_CJA_MovimientosCaja",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", parametros.Sucursal);
                    cmd.Parameters.AddWithValue("@nCaja", parametros.Caja);
                    cmd.Parameters.AddWithValue("@nTipoRegistroCaja", parametros.TipoRegistroCaja);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int ConceptoCajaIndex = reader.GetOrdinal("cConceptoCaja");

                        while (await reader.ReadAsync())
                        {
                            movimientos.Add(
                                new RegMovimientoCaja()
                                {
                                    Consecutivo = ConvertUtils.ToInt32(reader["nFolio"]),
                                    IDRegistroCaja = ConvertUtils.ToInt64(reader["nIDRegistroCaja"]),
                                    Concepto = reader.IsDBNull(ConceptoCajaIndex) ? "" : reader.GetString(ConceptoCajaIndex),
                                    Importe = ConvertUtils.ToDecimal(reader["nImporte"]),
                                    Fecha = ConvertUtils.ToDateTime(reader["dFecha"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    Hora = ConvertUtils.ToString(reader["cHora"]),
                                    Observaciones = ConvertUtils.ToString(reader["cComentarios"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los movimientos de caja")
                {
                    Metodo = "ObtenMovimientosCaja",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return movimientos;
        }

        public async Task<Decimal> ObtenImporteDisponibleCaja(ParametrosConsultaMovimientosCaja parametros)
        {
            Decimal disponible=0;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_ObtenImporteDisponibleApertura_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nIDApertura", parametros.IDApertura);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int ConceptoCajaIndex = reader.GetOrdinal("cConceptoCaja");

                        while (await reader.ReadAsync())
                        {
                            disponible = ConvertUtils.ToDecimal(reader["nDisponible"]);
                            
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
                throw new DataAccessException("Error(rp) No se pudo obtener disponible de caja")
                {
                    Metodo = "ObtenImporteDisponibleCaja",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return disponible;
        }
    }
}