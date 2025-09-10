using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatPlazasRepositorio : ICatPlazasRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatPlazasRepositorio> _logger;

        public CatPlazasRepositorio(ILogger<CatPlazasRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatPlaza> ObtenerPorPlaza(int nPlaza)
        {
            CatPlaza plaza = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatPlazas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nEmpresa", nPlaza);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            plaza =
                                new CatPlaza()
                                {
                                    plaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    region = ConvertUtils.ToInt32(reader["nRegion"]),
                                    descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    maquina = ConvertUtils.ToString(reader["cMaquina"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener la plaza")
                {
                    Metodo = "ObtenerPorPlaza",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return plaza;
        }

        public async Task<List<CatPlaza>> Lista()
        {
            List<CatPlaza> plazas = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatPlazas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nPlaza", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            plazas.Add(
                                new CatPlaza()
                                {
                                    plaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    region = ConvertUtils.ToInt32(reader["nRegion"]),
                                    descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    maquina = ConvertUtils.ToString(reader["cMaquina"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener las plazas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return plazas;
        }
    }
}
