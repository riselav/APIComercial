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
    public class RegCorteCajaRepositorio : IRegCorteCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegCorteCajaRepositorio> _logger;
        private readonly IRegMovimientoCajaRepositorio _movimientoCajaRepositorio;
        private readonly IRegAperturaCajaRepositorio _regAperturaCajaRepositorio;

        public RegCorteCajaRepositorio(ILogger<RegCorteCajaRepositorio> logger, Conexion conexion, 
                                          IRegMovimientoCajaRepositorio movimientoCajaRepositorio,
                                          IRegAperturaCajaRepositorio regAperturaCajaRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _movimientoCajaRepositorio = movimientoCajaRepositorio;
            _regAperturaCajaRepositorio = regAperturaCajaRepositorio;
        }

        private RegMovimientoCaja CrearMovimientoCaja(RegCorteCaja regCorteCaja,RegAperturaCaja regAperturaCaja)
        {
            RegMovimientoCaja regMovimientoCaja = new RegMovimientoCaja
            {
                IDRegistroCaja = regCorteCaja.IDCorte,
                TipoRegistroCaja = 6, //Corte de caja
                Sucursal = regCorteCaja.IDSucursal,
                Fecha = regCorteCaja.Fecha,
                Importe = 0,
                EmpleadoInvolucrado = regCorteCaja.IDUsuarioAutoriza == 0 ? null : regCorteCaja.IDUsuarioAutoriza,                
                Usuario = regCorteCaja.Usuario,
                Maquina = regCorteCaja.Maquina,
                IDApertura = regAperturaCaja.IDApertura
            };
            if (regCorteCaja.listRegCorteCajaDetalle != null && regCorteCaja.listRegCorteCajaDetalle.Count > 0)
            {
                regMovimientoCaja.Detalle = new List<RegMovimientoCajaDetalle>();
                foreach (RegCorteCajaDetalle det in regCorteCaja.listRegCorteCajaDetalle)
                {
                    RegMovimientoCajaDetalle detalle = new RegMovimientoCajaDetalle
                    {
                        IDRegistroCaja = regMovimientoCaja.IDRegistroCaja,
                        Renglon=det.Renglon,
                        FormaPago = det.FormaPago,
                        Importe = det.ImporteUsuario,
                        Usuario = regCorteCaja.Usuario,
                        Maquina = regCorteCaja.Maquina
                    };
                    if(det.FormaPago == 1) 
                    {
                        if (regCorteCaja.listRegDenominacionCorteCaja != null && regCorteCaja.listRegDenominacionCorteCaja.Count > 0)
                        {
                            detalle.DetalleDenominacion ??= new List<RegMovimientoCajaDetalleDenominacion>();
                            foreach (RegDenominacionCorteCaja detNom in regCorteCaja.listRegDenominacionCorteCaja)
                            {
                                if (detNom.Cantidad == 0)
                                    continue; 
                                RegMovimientoCajaDetalleDenominacion detalleNominacion = new RegMovimientoCajaDetalleDenominacion
                                {
                                    IDRegistroCaja = regMovimientoCaja.IDRegistroCaja,
                                    Renglon = detNom.Renglon,
                                    Importe = detNom.Importe,
                                    Denominacion = detNom.Denominacion,
                                    Valor = detNom.Valor ?? 0,
                                    Cantidad = (int)detNom.Cantidad,
                                    Usuario = regCorteCaja.Usuario,
                                    Maquina = regCorteCaja.Maquina
                                };
                                detalle.DetalleDenominacion.Add(detalleNominacion);
                            }
                        }
                    }
                    regMovimientoCaja.Detalle.Add(detalle);
                }
            }
            
            return regMovimientoCaja;
        }

        public async Task<RegCorteCaja> IME_REG_CorteCaja(RegCorteCaja regCorteCaja)
        {
            RegAperturaCaja paramApertura = new RegAperturaCaja
            {
                IDCaja = regCorteCaja.IDCaja,
                IDSucursal = regCorteCaja.IDSucursal
            };
            RegAperturaCaja apertura = await _regAperturaCajaRepositorio.ObtenAperturaAbierta(paramApertura);
            using (var con = _conexion.ObtenerSqlConexion())
            {
                await con.OpenAsync();

                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand()
                        {
                            Connection = con,
                            Transaction = transaction,
                            CommandText = "RST_IME_CAJ_CorteCaja",
                            CommandType = CommandType.StoredProcedure,
                        };
                        
                        SqlParameter outputParam = new SqlParameter("@nConsecutivo", SqlDbType.BigInt);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        cmd.Parameters.AddWithValue("@nFolio", 0);
                        cmd.Parameters.AddWithValue("@nSucursal", regCorteCaja.IDSucursal);
                        cmd.Parameters.AddWithValue("@nCaja", regCorteCaja.IDCaja);
                        cmd.Parameters.AddWithValue("@dFechaCorte", regCorteCaja.Fecha);

                        cmd.Parameters.AddWithValue("@nUsuarioAutoriza", regCorteCaja.IDUsuarioAutoriza == 0 ? null : regCorteCaja.IDUsuarioAutoriza);

                        cmd.Parameters.AddWithValue("@bActivo", regCorteCaja.Activo);

                        cmd.Parameters.AddWithValue("@cUsuario", regCorteCaja.Usuario);
                        cmd.Parameters.AddWithValue("@cNombreMaquina", regCorteCaja.Maquina);

                        await cmd.ExecuteNonQueryAsync();

                        var consecutivo= (long)cmd.Parameters["@nConsecutivo"].Value;
                        var idCorte = GenerarFolio(regCorteCaja.IDSucursal, consecutivo);

                      regCorteCaja.IDCorte = long.Parse(idCorte);

                        if (regCorteCaja.IDCorte > 0 && regCorteCaja.listRegCorteCajaDetalle != null && regCorteCaja.listRegCorteCajaDetalle.Count > 0)
                        {
                            int renglon = 1; 
                            foreach (RegCorteCajaDetalle det in regCorteCaja.listRegCorteCajaDetalle)
                            {
                                
                                SqlCommand cmdInsDetalle = new SqlCommand()
                                {
                                    Connection = con,
                                    Transaction = transaction,
                                    CommandText = "RST_IME_CAJ_DetalleCorteCaja",
                                    CommandType = CommandType.StoredProcedure,
                                };
                                det.IDCorte = regCorteCaja.IDCorte;
                                det.Renglon = renglon;
                                cmdInsDetalle.Parameters.AddWithValue("@nIDCorteCaja", regCorteCaja.IDCorte);
                                cmdInsDetalle.Parameters.AddWithValue("@nRenglon", renglon);
                                cmdInsDetalle.Parameters.AddWithValue("@nFormaPago", det.FormaPago);
                                cmdInsDetalle.Parameters.AddWithValue("@nImporteCorte", det.ImporteSistema);
                                cmdInsDetalle.Parameters.AddWithValue("@nImporteUsuario", det.ImporteUsuario);
                                cmdInsDetalle.Parameters.AddWithValue("@cUsuario", regCorteCaja.Usuario);
                                cmdInsDetalle.Parameters.AddWithValue("@cMaquina", regCorteCaja.Maquina);
                                await cmdInsDetalle.ExecuteNonQueryAsync();
                                renglon++;

                            }
                            
                        }

                        if (regCorteCaja.IDCorte > 0 && regCorteCaja.listRegDenominacionCorteCaja != null && regCorteCaja.listRegDenominacionCorteCaja.Count > 0)
                        {
                            int renglon = 1;
                            foreach (RegDenominacionCorteCaja det in regCorteCaja.listRegDenominacionCorteCaja)
                            {
                                if(det.Cantidad == 0 )
                                    continue; 
                                SqlCommand cmdInsDetalle = new SqlCommand()
                                {
                                    Connection = con,
                                    Transaction = transaction,
                                    CommandText = "RST_IME_CAJ_DetalleDenominacionCortesCaja",
                                    CommandType = CommandType.StoredProcedure,
                                };
                                cmdInsDetalle.Parameters.AddWithValue("@nFolio", regCorteCaja.IDCorte);
                                cmdInsDetalle.Parameters.AddWithValue("@nRenglon", renglon);
                                cmdInsDetalle.Parameters.AddWithValue("@nDenominacion", det.Denominacion);
                                cmdInsDetalle.Parameters.AddWithValue("@nValor", det.Valor);
                                cmdInsDetalle.Parameters.AddWithValue("@nCantidad", det.Cantidad);
                                cmdInsDetalle.Parameters.AddWithValue("@nImporte", det.Importe);                                
                                cmdInsDetalle.Parameters.AddWithValue("@cUsuario", regCorteCaja.Usuario);
                                cmdInsDetalle.Parameters.AddWithValue("@cMaquina", regCorteCaja.Maquina);
                                await cmdInsDetalle.ExecuteNonQueryAsync();
                                det.Renglon = renglon;
                                
                                renglon++;

                            }

                        }

                        var regMovimientoCaja = CrearMovimientoCaja(regCorteCaja,apertura);
                        
                        await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regMovimientoCaja, con, transaction);


                        int returnValue = -1;
                        SqlCommand cmdActualizaMovimiento = new SqlCommand()
                        {
                            Connection = con,
                            Transaction = transaction,
                            CommandText = "RST_UP_ActualizaMovimientosCorteCaja",
                            CommandType = CommandType.StoredProcedure,
                        };
                        cmdActualizaMovimiento.Parameters.AddWithValue("@nIDCorteCaja", regCorteCaja.IDCorte);                        
                        cmdActualizaMovimiento.Parameters.AddWithValue("@nIDApertura", apertura.IDApertura);
                        cmdActualizaMovimiento.Parameters.AddWithValue("@bActivo", 1);
                        cmdActualizaMovimiento.Parameters.AddWithValue("@cUsuario", regCorteCaja.Usuario);
                        cmdActualizaMovimiento.Parameters.AddWithValue("@cNombreMaquina", regCorteCaja.Maquina);

                        SqlParameter retValueParam = new SqlParameter
                        {
                            ParameterName = "@ReturnValue", 
                            Direction = ParameterDirection.ReturnValue,
                            SqlDbType = SqlDbType.Int 
                        };
                        cmdActualizaMovimiento.Parameters.Add(retValueParam);

                        try
                        {
                            await cmdActualizaMovimiento.ExecuteNonQueryAsync();
                            if (retValueParam.Value != DBNull.Value)
                            {
                                returnValue = (int)retValueParam.Value;
                            }
                            if(returnValue<0)
                            {
                                throw new Exception("Ocurrio un error al actualizar movimientos de corte de caja");
                            }
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine($"SQL Error: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"General Error: {ex.Message}");
                            throw;
                        }
                        
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                        string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                        int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                        _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                        throw new DataAccessException("Error(rp) al insertar apertura de caja")
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

            return regCorteCaja;
        }
        public string GenerarFolio(int nSucursal, long nConsecutivo)
        {           
            string sucursalPadded = nSucursal.ToString().PadLeft(5, '0');
            string consecutivoPadded = nConsecutivo.ToString().PadLeft(8, '0');
            string nFolio = "1" + sucursalPadded + consecutivoPadded;
            return nFolio;
        }

        public async Task<List<ImpresionData>> TicketCorteCaja(int idSucursal, long idCorte)
        {
            List<ImpresionData> impresion = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_TicketCorteCaja",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", idSucursal);
                    cmd.Parameters.AddWithValue("@FolioCorte", idCorte);
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
                                    cLinea = ConvertUtils.ToString(reader["cLinea"]),
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
    }
}