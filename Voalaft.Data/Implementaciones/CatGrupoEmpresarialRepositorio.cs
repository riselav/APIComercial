
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
    public class CatGrupoEmpresarialRepositorio : ICatGrupoEmpresarialRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatGrupoEmpresarialRepositorio> _logger;

        public CatGrupoEmpresarialRepositorio(ILogger<CatGrupoEmpresarialRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatGrupoEmpresarial> IME_CatGrupoEmpresarial(CatGrupoEmpresarial catGrupoEmpresarial)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatGrupoEmpresarial",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nGrupoEmpresarial", catGrupoEmpresarial.GrupoEmpresarial);
                    cmd.Parameters.AddWithValue("@cDescripcion", catGrupoEmpresarial.Descripcion);
                    cmd.Parameters.AddWithValue("@bActivo", catGrupoEmpresarial.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catGrupoEmpresarial.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catGrupoEmpresarial.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los grupos empresariales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catGrupoEmpresarial;
        }

        public async Task<CatGrupoEmpresarial> ObtenerPorGrupoEmpresarial(int nGrupoEmpresarial)
        {
            CatGrupoEmpresarial linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatGrupoEmpresarial",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nGrupoEmpresarial", nGrupoEmpresarial);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatGrupoEmpresarial()
                                {
                                    GrupoEmpresarial = ConvertUtils.ToInt32(reader["nGrupoEmpresarial"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los grupos empresariales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return linea;
        }

        public async Task<List<CatGrupoEmpresarial>> Lista()
        {
            List<CatGrupoEmpresarial> GrupoEmpresarial = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatGrupoEmpresarial",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nGrupoEmpresarial", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            GrupoEmpresarial.Add(
                                new CatGrupoEmpresarial()
                                {
                                    GrupoEmpresarial = ConvertUtils.ToInt32(reader["nGrupoEmpresarial"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los grupos empresariales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return GrupoEmpresarial;
        }

        
    }
}
