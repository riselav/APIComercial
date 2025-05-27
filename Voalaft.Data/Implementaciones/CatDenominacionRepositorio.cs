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
    public class CatDenominacionRepositorio: ICatDenominacionRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatDenominacionRepositorio> _logger;

        public CatDenominacionRepositorio(ILogger<CatDenominacionRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatDenominacion> ObtenerPorId(long n_Denominacion)
        {
            CatDenominacion catDenominacion = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Denominaciones_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nDenominacion", n_Denominacion);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int ImagenIndex = reader.GetOrdinal("cImagen");

                        while (await reader.ReadAsync())
                        {
                            catDenominacion =
                                new CatDenominacion()
                                {
                                    Denominacion = ConvertUtils.ToInt32(reader["nDenominacion"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Tipo = ConvertUtils.ToInt32(reader["nTipo"]),
                                    cImagen = reader.IsDBNull(ImagenIndex) ? "" : reader.GetString(ImagenIndex),
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
                throw new DataAccessException("Error(rp) No se pudo obtener denominacion")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catDenominacion;
        }

        public async Task<List<CatDenominacion>> Lista()
        {
            List<CatDenominacion> denominacions = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Denominaciones_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nDenominacion", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int ImagenIndex = reader.GetOrdinal("cImagen");

                        while (await reader.ReadAsync())
                        {
                            denominacions.Add(
                                new CatDenominacion()
                                {
                                    Denominacion = ConvertUtils.ToInt32(reader["nDenominacion"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Tipo = ConvertUtils.ToInt32(reader["nTipo"]),
                                    cImagen = reader.IsDBNull(ImagenIndex) ? "" : reader.GetString(ImagenIndex),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de las denominaciones")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return denominacions;
        }
    }
}
