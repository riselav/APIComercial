
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
    public class CatProductosBaseRepositorio : ICatProductosBaseRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ICatalagosSATRepositorio _catalagosSATRepositorio;
        private readonly ILogger<CatProductosBaseRepositorio> _logger;

        public CatProductosBaseRepositorio(ILogger<CatProductosBaseRepositorio> logger,Conexion conexion, ICatalagosSATRepositorio catalagosSATRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _catalagosSATRepositorio = catalagosSATRepositorio;
        }

        public async Task<CatProductosBase> IME_CatProductosBase(CatProductosBase catProductoBase)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatProductosBase",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nProductoBase", catProductoBase.ProductoBase);                    
                    cmd.Parameters.AddWithValue("@cDescripcion", catProductoBase.Descripcion);
                    cmd.Parameters.AddWithValue("@nTipoUnidad", catProductoBase.TipoUnidad);
                    cmd.Parameters.AddWithValue("@bActivo", catProductoBase.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catProductoBase.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catProductoBase.Maquina);

                    SqlParameter returnValue = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    catProductoBase.ProductoBase = (int)returnValue.Value;
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

            return catProductoBase;
        }

        public async Task<CatProductosBase> ObtenerPorProductoBase(int nProductoBase)
        {
            CatProductosBase linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatProductosBase",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nProductoBase", nProductoBase);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatProductosBase()
                                {
                                    ProductoBase = ConvertUtils.ToInt32(reader["nProductoBase"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    TipoUnidad = ConvertUtils.ToInt16(reader["nTipoUnidad"]),
                                    DescripcionTipoUnidad = ConvertUtils.ToString(reader["DescripcionTipoUnidad"]),
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

        public async Task<List<CatProductosBase>> Lista()
        {
            List<CatProductosBase> ProductosBase = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatProductosBase",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nProductoBase", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ProductosBase.Add(
                                new CatProductosBase()
                                {
                                    ProductoBase = ConvertUtils.ToInt32(reader["nProductoBase"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    TipoUnidad = ConvertUtils.ToInt16(reader["nTipoUnidad"]),
                                    DescripcionTipoUnidad = ConvertUtils.ToString(reader["DescripcionTipoUnidad"]),
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

            return ProductosBase;
        }

        
    }
}
