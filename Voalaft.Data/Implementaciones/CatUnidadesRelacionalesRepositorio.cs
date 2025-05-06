
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatUnidadesRelacionalesRepositorio : ICatUnidadesRelacionalesRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ICatalagosSATRepositorio _catalagosSATRepositorio;
        private readonly ILogger<CatUnidadesRelacionalesRepositorio> _logger;

        public CatUnidadesRelacionalesRepositorio(ILogger<CatUnidadesRelacionalesRepositorio> logger,Conexion conexion, ICatalagosSATRepositorio catalagosSATRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _catalagosSATRepositorio = catalagosSATRepositorio;
        }

        public async Task<CatUnidadesRelacionales> IME_CatUnidadesRelacionales(CatUnidadesRelacionales catUnidadRelacional)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatUnidadesRelacionales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidadRelacional", catUnidadRelacional.UnidadRelacional);                    
                    cmd.Parameters.AddWithValue("@cDescripcion", catUnidadRelacional.Descripcion);
                    cmd.Parameters.AddWithValue("@nTipoUnidad", catUnidadRelacional.TipoUnidad);
                    cmd.Parameters.AddWithValue("@bEsBase", catUnidadRelacional.EsBase);
                    cmd.Parameters.AddWithValue("@nEquivalencia", catUnidadRelacional.Equivalencia);
                    cmd.Parameters.AddWithValue("@bActivo", catUnidadRelacional.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catUnidadRelacional.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catUnidadRelacional.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catUnidadRelacional.Linea = folioSig;
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los UnidadRelacional")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catUnidadRelacional;
        }

        public async Task<CatUnidadesRelacionales> ObtenerPorUnidadRelacional(int nUnidadRelacional)
        {
            CatUnidadesRelacionales linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidadesRelacionales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidadRelacional", nUnidadRelacional);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatUnidadesRelacionales()
                                {
                                    UnidadRelacional = ConvertUtils.ToInt32(reader["nUnidadRelacional"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    TipoUnidad = ConvertUtils.ToInt16(reader["nTipoUnidad"]),
                                    DescripcionTipoUnidad = ConvertUtils.ToString(reader["DescripcionTipoUnidad"]),
                                    EsBase= ConvertUtils.ToBoolean(reader["bEsBase"]),
                                    Equivalencia= ConvertUtils.ToDecimal(reader["nEquivalencia"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los UnidadRelacional")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return linea;
        }

        public async Task<List<CatUnidadesRelacionales>> Lista()
        {
            List<CatUnidadesRelacionales> UnidadesRelacionales = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidadesRelacionales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nUnidadRelacional", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            UnidadesRelacionales.Add(
                                new CatUnidadesRelacionales()
                                {
                                    UnidadRelacional = ConvertUtils.ToInt32(reader["nUnidadRelacional"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    TipoUnidad = ConvertUtils.ToInt16(reader["nTipoUnidad"]),
                                    DescripcionTipoUnidad = ConvertUtils.ToString(reader["DescripcionTipoUnidad"]),
                                    EsBase = ConvertUtils.ToBoolean(reader["bEsBase"]),
                                    Equivalencia = ConvertUtils.ToDecimal(reader["nEquivalencia"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los UnidadRelacional")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return UnidadesRelacionales;
        }

        
    }
}
