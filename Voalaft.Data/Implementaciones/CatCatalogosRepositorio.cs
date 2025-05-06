
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
    public class CatCatalogosRepositorio : ICatCatalogosRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatCatalogosRepositorio> _logger;

        public CatCatalogosRepositorio(ILogger<CatCatalogosRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        
        public async Task<List<CatCatalogos>> ObtenerPorNombre(string cNombre)
        {
            List<CatCatalogos> cat = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatCatalogos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cNombre", cNombre);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cat.Add(
                                new CatCatalogos()
                                {
                                    Catalogo = ConvertUtils.ToInt32(reader["nCatalogo"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
                                    Codigo = ConvertUtils.ToInt32(reader["nCodigo"]),
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
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "ObtenerPorNombre",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cat;
        }

        public async Task<List<CatCatalogos>> Lista()
        {
            List<CatCatalogos> cat = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatCatalogos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cNombre", "");
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cat.Add(
                                new CatCatalogos()
                                {
                                    Catalogo = ConvertUtils.ToInt32(reader["nCatalogo"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
                                    Codigo = ConvertUtils.ToInt32(reader["nCodigo"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cat;
        }

        
    }
}
