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
    public class CatFormaPagoRepositorio:ICatFormaPagoRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatFormaPagoRepositorio> _logger;

        public CatFormaPagoRepositorio(ILogger<CatFormaPagoRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatFormaPago> ObtenerPorId(long n_FormaPago)
        {
            CatFormaPago catFormaPago = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_FormasPago_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFormaPago", n_FormaPago);
                    cmd.Parameters.AddWithValue("@nTipoIngreso", -1);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catFormaPago =
                                new CatFormaPago()
                                {
                                    FormaPago = ConvertUtils.ToInt32(reader["nFormaPago"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Ingreso = ConvertUtils.ToBoolean(reader["bIngreso"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario= ConvertUtils.ToString(reader["cUsuario_Registro"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registro"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener forma de pago")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catFormaPago;
        }

        public async Task<List<CatFormaPago>> Lista()
        {
            List<CatFormaPago> formasPago = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_FormasPago_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFormaPago", 0);
                    cmd.Parameters.AddWithValue("@nTipoIngreso", -1);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            formasPago.Add(
                                new CatFormaPago()
                                {
                                    FormaPago = ConvertUtils.ToInt32(reader["nFormaPago"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Ingreso = ConvertUtils.ToBoolean(reader["bIngreso"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registro"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registro"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de formas de pago")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return formasPago;
        }

        public async Task<List<CatFormaPago>> ObtenerPorTipoEgreso(int n_TipoEgreso)
        {
            List<CatFormaPago> formasPago = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_FormasPago_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFormaPago", 0);
                    cmd.Parameters.AddWithValue("@nTipoIngreso", n_TipoEgreso);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            formasPago.Add(
                                new CatFormaPago()
                                {
                                    FormaPago = ConvertUtils.ToInt32(reader["nFormaPago"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Ingreso = ConvertUtils.ToBoolean(reader["bIngreso"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registro"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registro"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de formas de pago por tipo de egreso")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return formasPago;
        }
    }
}
