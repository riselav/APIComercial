
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
    public class CatUnidadesRepositorio : ICatUnidadesRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatUnidadesRepositorio> _logger;

        public CatUnidadesRepositorio(ILogger<CatUnidadesRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatUnidades> IME_CatUnidades(CatUnidades catUnidad)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatUnidades",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidad", catUnidad.Unidad);
                    cmd.Parameters.AddWithValue("@cDescripcion", catUnidad.Descripcion);
                    cmd.Parameters.AddWithValue("@nTipoUnidad", catUnidad.TipoUnidad);                    
                    cmd.Parameters.AddWithValue("@cClaveSAT", catUnidad.ClaveSAT);
                    cmd.Parameters.AddWithValue("@cAbreviatura", catUnidad.Abreviatura);
                    cmd.Parameters.AddWithValue("@bActivo", catUnidad.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catUnidad.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catUnidad.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catUnidad.TipoUnidad = folioSig;
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los Unidades")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catUnidad;
        }

        public async Task<CatUnidades> ObtenerPorUnidad(int nUnidad)
        {
            CatUnidades linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidades",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidad", nUnidad);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatUnidades()
                                {
                                    Unidad = ConvertUtils.ToInt32(reader["nUnidad"]),
                                    TipoUnidad = ConvertUtils.ToInt32(reader["nTipoUnidad"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Abreviatura = ConvertUtils.ToString(reader["cAbreviatura"]),
                                    ClaveSAT = ConvertUtils.ToString(reader["cClaveSAT"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"])
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

            return linea;
        }

        public async Task<List<CatUnidades>> Lista()
        {
            List<CatUnidades> Unidades = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidades",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidad", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Unidades.Add(
                                new CatUnidades()
                                {
                                    Unidad = ConvertUtils.ToInt32(reader["nUnidad"]),
                                    TipoUnidad = ConvertUtils.ToInt32(reader["nTipoUnidad"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Abreviatura = ConvertUtils.ToString(reader["cAbreviatura"]),
                                    ClaveSAT = ConvertUtils.ToString(reader["cClaveSAT"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"])
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

            return Unidades;
        }

        
    }
}
