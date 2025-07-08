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
    public class CatCajaRepositorio:ICatCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatCajaRepositorio> _logger;

        public CatCajaRepositorio(ILogger<CatCajaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<List<CatCaja>> ObtenerPorSucursal(long n_Sucursal)
        {
            List<CatCaja> catCajas = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CajasSucursal_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCaja", 0);
                    cmd.Parameters.AddWithValue("@nSucursal", n_Sucursal);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catCajas.Add(
                                new CatCaja()
                                {
                                    Caja = ConvertUtils.ToInt32(reader["nCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Sucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    NombreSucursal = ConvertUtils.ToString(reader["cNombreSucursal"]),
                                    Impresora = ConvertUtils.ToInt32(reader["nImpresora"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener las cajas de una sucursal")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catCajas;
        }

        public async Task<List<CatCaja>> Lista()
        {
            List<CatCaja> catCajas = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CajasSucursal_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCaja", 0);
                    cmd.Parameters.AddWithValue("@nSucursal", 0);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catCajas.Add(
                                new CatCaja()
                                {
                                    Caja = ConvertUtils.ToInt32(reader["nCaja"]),
                                    Sucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener el listado de cajas de sucursales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catCajas;
        }

        public async Task<CatCaja> ObtenerCajaPorId(long n_Caja)
        {
            CatCaja catCaja = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CajasSucursal_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCaja", n_Caja);
                    cmd.Parameters.AddWithValue("@nSucursal", 0);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catCaja=(
                                new CatCaja()
                                {
                                    Caja = ConvertUtils.ToInt32(reader["nCaja"]),
                                    Sucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"])
                                }
                                );
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
                throw new DataAccessException("Error(rp) No se pudo obtener el listado de cajas de sucursales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catCaja;
        }

        public async Task<CatCaja> IME_Caja(CatCaja caja)
        {
            CatCaja caj = caja;

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
                            CommandText = "CAT_IME_Cajas",
                            CommandType = CommandType.StoredProcedure,
                        };
                        cmd.Parameters.AddWithValue("@nFolio", caja.Caja);
                        cmd.Parameters.AddWithValue("@nsucursal", caja.Sucursal);
                        cmd.Parameters.AddWithValue("@cDescripcion", caja.Descripcion);
                        cmd.Parameters.AddWithValue("@nImpresora", caja.Impresora);
                        
                        cmd.Parameters.AddWithValue("@bActivo", caja.Activo);
                        cmd.Parameters.AddWithValue("@cUsuario", caja.Usuario);
                        cmd.Parameters.AddWithValue("@cNombreMaquina ", caja.Maquina);
      
                        //SqlParameter outputParam = new SqlParameter("@nVenta", SqlDbType.BigInt);
                        //outputParam.Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add(outputParam);

                        // Agrega el parámetro de retorno
                        var returnParameter = new SqlParameter
                        {
                            ParameterName = "@RETURN_VALUE",
                            Direction = ParameterDirection.ReturnValue
                        };
                        cmd.Parameters.Add(returnParameter);

                        await cmd.ExecuteNonQueryAsync();

                        //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;

                        //long valorOutput = (long)cmd.Parameters["@nVenta"].Value;

                        bool nvo = false;
                        int folioSig = (int)(returnParameter.Value ?? 0);

                        if (caj.Caja == 0)
                        {
                            nvo = true;
                            caj.Caja = folioSig;
                        }                 

                        // Todo bien, commit
                        if (caj != null)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        caj = null;

                        string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                        string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                        int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                        _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                        throw new DataAccessException("Error(rp) al insertar/editar caja de sucursal")
                        {
                            Metodo = "IME_Caja",
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

            return caj;
        }
    }
}