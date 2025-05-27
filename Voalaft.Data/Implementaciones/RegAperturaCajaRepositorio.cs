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
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class RegAperturaCajaRepositorio:IRegAperturaCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegAperturaCajaRepositorio> _logger;
        private readonly IRegMovimientoCajaRepositorio _movimientoCajaRepositorio;

        public RegAperturaCajaRepositorio(ILogger<RegAperturaCajaRepositorio> logger, Conexion conexion, 
                                          IRegMovimientoCajaRepositorio movimientoCajaRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _movimientoCajaRepositorio = movimientoCajaRepositorio;
        }

        public async Task<RegAperturaCaja> IME_REG_AperturaCaja(RegAperturaCaja regAperturaCaja)
        {
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
                            CommandText = "CM_IME_CAJ_RegistroApertura",
                            CommandType = CommandType.StoredProcedure,
                        };
                        
                        SqlParameter outputParam = new SqlParameter("@nFolio", SqlDbType.BigInt);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        cmd.Parameters.AddWithValue("@nSucursal", regAperturaCaja.IDSucursal);
                        cmd.Parameters.AddWithValue("@nCaja", regAperturaCaja.IDCaja);
                        cmd.Parameters.AddWithValue("@nTurno", regAperturaCaja.IDTurno);
                        cmd.Parameters.AddWithValue("@dFecha", regAperturaCaja.Fecha);
                                                
                        cmd.Parameters.AddWithValue("@nDotacionInicial", regAperturaCaja.DotacionInicial);
                        cmd.Parameters.AddWithValue("@nEmpleado", regAperturaCaja.IDEmpleado);
                        cmd.Parameters.AddWithValue("@nUsuarioAutoriza", regAperturaCaja.IDUsuarioAutoriza == 0 ? null : regAperturaCaja.IDUsuarioAutoriza);

                        cmd.Parameters.AddWithValue("@nEstatus", regAperturaCaja.Estatus);
                        cmd.Parameters.AddWithValue("@bActivo", regAperturaCaja.Activo);

                        cmd.Parameters.AddWithValue("@cUsuario", regAperturaCaja.Usuario);
                        cmd.Parameters.AddWithValue("@cNombreMaquina", regAperturaCaja.Maquina);

                        //SqlParameter outputParam = new SqlParameter("@nVenta", SqlDbType.BigInt);
                        //outputParam.Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add(outputParam);

                        await cmd.ExecuteNonQueryAsync();

                        //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                        //catMarca.Marca = folioSig;

                        regAperturaCaja.IDApertura = (long)cmd.Parameters["@nFolio"].Value;

                        if (regAperturaCaja.IDApertura > 0 && regAperturaCaja.regMovimientoCaja != null)
                        {
                            regAperturaCaja.regMovimientoCaja.IDApertura = regAperturaCaja.IDApertura;

                            await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regAperturaCaja.regMovimientoCaja,con, transaction);
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

            return regAperturaCaja;
        }

        public async Task<RegAperturaCaja> ObtenAperturaAbierta(RegAperturaCaja regAperturaCaja)
        {
            RegAperturaCaja aperturaCaja = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT * FROM CAJ_RegistrosAperturaCaja (NOLOCK) where " +
                        "bActivo=1 AND nIDSucursal=@nIDSucursal AND nIDCaja=@nIDCaja AND nEstatus=1", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@nIDSucursal", SqlDbType.Int).Value = regAperturaCaja.IDSucursal;
                    cmd.Parameters.Add("@nIDCaja", SqlDbType.Int).Value = regAperturaCaja.IDCaja;

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int UsuarioAutorizaIndex = reader.GetOrdinal("nIDUsuarioAutoriza");
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            aperturaCaja =
                                new RegAperturaCaja()
                                {
                                    IDApertura = ConvertUtils.ToInt64(reader["nIDApertura"]),
                                    IDSucursal = ConvertUtils.ToInt32(reader["nIDSucursal"]),
                                    IDCaja = ConvertUtils.ToInt32(reader["nIDCaja"]),
                                    IDTurno = ConvertUtils.ToInt32(reader["nIDTurno"]),
                                    Fecha = ConvertUtils.ToDateTime(reader["dFecha"]),
                                    DotacionInicial= ConvertUtils.ToDecimal(reader["nDotacionInicial"]),
                                    IDEmpleado = ConvertUtils.ToInt32(reader["nIDEmpleado"]),
                                    IDUsuarioAutoriza = reader.IsDBNull(UsuarioAutorizaIndex) ? 0 : reader.GetInt16(UsuarioAutorizaIndex),
                                    Estatus = ConvertUtils.ToInt32(reader["nEstatus"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"])
                                };
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
                throw new DataAccessException("Error(rp) No se pudo obtener la apertura abierta")
                {
                    Metodo = "ObtenAperturaAbierta",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return aperturaCaja;
        }
    }
}