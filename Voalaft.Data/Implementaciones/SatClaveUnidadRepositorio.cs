
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.ConstrainedExecution;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class SatClaveUnidadRepositorio : ISatClaveUnidadRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<SatClaveUnidadRepositorio> _logger;

        public SatClaveUnidadRepositorio(ILogger<SatClaveUnidadRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        
        public async Task<SatClaveUnidad> ObtenerPorClaveUnidad(string c_ClaveUnidad)
        {
            SatClaveUnidad tipoFactor = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_ClaveUnidad",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@c_ClaveUnidad", c_ClaveUnidad);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactor=
                                new SatClaveUnidad()
                                {
                                    c_ClaveUnidad = ConvertUtils.ToString(reader["c_ClaveUnidad"]),
                                    Nombre = ConvertUtils.ToString(reader["Nombre"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return tipoFactor;
        }

        public async Task<List<SatClaveUnidad>> Lista()
        {
            List<SatClaveUnidad> tipoFactors = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_ClaveUnidad",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@c_ClaveUnidad", string.Empty);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactors.Add(
                                new SatClaveUnidad()
                                {
                                    c_ClaveUnidad = ConvertUtils.ToString(reader["c_ClaveUnidad"]),
                                    Nombre = ConvertUtils.ToString(reader["Nombre"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] :"";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);
                
                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return tipoFactors;
        }

        
    }
}
