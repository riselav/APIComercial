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
    public class CatConceptoCajaRepositorio : ICatConceptoCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatConceptoCajaRepositorio> _logger;

        public CatConceptoCajaRepositorio(ILogger<CatConceptoCajaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatConceptoCaja> ObtenerPorId(long n_FormaPago)
        {
            CatConceptoCaja catConceptoCaja = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_ConceptosCaja_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nConceptoCaja", n_FormaPago);
                    cmd.Parameters.AddWithValue("@nEfecto", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catConceptoCaja =
                                new CatConceptoCaja()
                                {
                                    ConceptoCaja = ConvertUtils.ToInt32(reader["nConceptoCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Efecto = ConvertUtils.ToInt32(reader["nEfecto"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registro"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener concepto de caja")
                {
                    Metodo = "ObtenerPorId",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catConceptoCaja;
        }

        public async Task<List<CatConceptoCaja>> Lista()
        {
            List<CatConceptoCaja> conceptosCaja = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_ConceptosCaja_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nConceptoCaja", 0);
                    cmd.Parameters.AddWithValue("@nEfecto", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            conceptosCaja.Add(
                                new CatConceptoCaja()
                                {
                                    ConceptoCaja = ConvertUtils.ToInt32(reader["nConceptoCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Efecto = ConvertUtils.ToInt32(reader["nEfecto"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de conceptos de caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return conceptosCaja;
        }

        public async Task<List<CatConceptoCaja>> ObtenerPorEfecto(int n_Efecto)
        {
            List<CatConceptoCaja> conceptosCaja = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_ConceptosCaja_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nConceptoCaja", 0);
                    cmd.Parameters.AddWithValue("@nEfecto", n_Efecto);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            conceptosCaja.Add(
                                new CatConceptoCaja()
                                {
                                    ConceptoCaja = ConvertUtils.ToInt32(reader["nConceptoCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Efecto = ConvertUtils.ToInt32(reader["nEfecto"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de conceptos de caja por efecto")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return conceptosCaja;
        }
    }
}