
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
    public class CatSublineasRepositorio : ICatSublineasRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatSublineasRepositorio> _logger;

        public CatSublineasRepositorio(ILogger<CatSublineasRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatSublineas> IME_CatSublineas(CatSublineas catSublinea)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatSublineas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSublinea", catSublinea.Sublinea);
                    cmd.Parameters.AddWithValue("@nLinea", catSublinea.Linea);
                    cmd.Parameters.AddWithValue("@cDescripcion", catSublinea.Descripcion);
                    cmd.Parameters.AddWithValue("@bActivo", catSublinea.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catSublinea.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catSublinea.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catSublinea.Linea = folioSig;
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

            return catSublinea;
        }

        public async Task<CatSublineas> ObtenerPorSublinea(int nSubLinea)
        {
            CatSublineas linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatSublineas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSubLinea", nSubLinea);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatSublineas()
                                {
                                    Sublinea = ConvertUtils.ToInt32(reader["nSublinea"]),
                                    Linea = ConvertUtils.ToInt32(reader["nLinea"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    DescripcionLinea = ConvertUtils.ToString(reader["cDescripcionLinea"]),
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

        public async Task<List<CatSublineas>> Lista()
        {
            List<CatSublineas> Sublineas = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatSublineas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSublinea", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Sublineas.Add(
                                new CatSublineas()
                                {
                                    Sublinea = ConvertUtils.ToInt32(reader["nSublinea"]),
                                    Linea = ConvertUtils.ToInt32(reader["nLinea"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    DescripcionLinea = ConvertUtils.ToString(reader["cDescripcionLinea"]),
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

            return Sublineas;
        }

        
    }
}
