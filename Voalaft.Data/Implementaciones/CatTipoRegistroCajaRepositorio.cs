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
    public class CatTipoRegistroCajaRepositorio: ICatTipoRegistroCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatTipoRegistroCajaRepositorio> _logger;

        public CatTipoRegistroCajaRepositorio(ILogger<CatTipoRegistroCajaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatTipoRegistroCaja> ObtenerPorId(int n_TipoRegistroCaja)
        {
            CatTipoRegistroCaja catTipoRegistroCaja = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_TiposRegistrosCaja_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTipoRegistroCaja", n_TipoRegistroCaja);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catTipoRegistroCaja =
                                new CatTipoRegistroCaja()
                                {
                                    TipoRegistroCaja = ConvertUtils.ToInt32(reader["nTipoRegistroCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Efecto = ConvertUtils.ToInt32(reader["Efecto"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener tipo de registro de caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catTipoRegistroCaja;
        }

        public async Task<List<CatTipoRegistroCaja>> Lista()
        {
            List<CatTipoRegistroCaja> catTipoRegistroCaja = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_TiposRegistrosCaja_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTipoRegistroCaja", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catTipoRegistroCaja.Add(
                                new CatTipoRegistroCaja()
                                {
                                    TipoRegistroCaja = ConvertUtils.ToInt32(reader["nTipoRegistroCaja"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Efecto = ConvertUtils.ToInt32(reader["Efecto"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de tipo de registro de caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catTipoRegistroCaja;
        }
    }
}