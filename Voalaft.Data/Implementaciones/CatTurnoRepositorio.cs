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
    public class CatTurnoRepositorio:ICatTurnoRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatTurnoRepositorio> _logger;

        public CatTurnoRepositorio(ILogger<CatTurnoRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatTurno> ObtenerPorId(long n_Turno)
        {
            CatTurno catTurno = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Turnos_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTurno", n_Turno);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catTurno =
                                new CatTurno()
                                {
                                    Turno = ConvertUtils.ToInt32(reader["nTurno"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    //nSucursalRegistro = reader.IsDBNull(SucursalRegistroIndex) ? 0 : reader.GetInt32(SucursalRegistroIndex),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener turnos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catTurno;
        }

        public async Task<List<CatTurno>> Lista()
        {
            List<CatTurno> turnos = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Turnos_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTurno", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            turnos.Add(
                                new CatTurno()
                                {
                                    Turno = ConvertUtils.ToInt32(reader["nTurno"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    //nSucursalRegistro = reader.IsDBNull(SucursalRegistroIndex) ? 0 : reader.GetInt32(SucursalRegistroIndex),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de los turnos")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return turnos;
        }

    }
}
