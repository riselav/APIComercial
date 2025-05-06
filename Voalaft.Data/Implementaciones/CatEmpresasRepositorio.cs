
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
    public class CatEmpresasRepositorio : ICatEmpresasRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatEmpresasRepositorio> _logger;

        public CatEmpresasRepositorio(ILogger<CatEmpresasRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatEmpresas> IME_CatEmpresas(CatEmpresas catEmpresa)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatEmpresas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nEmpresa", catEmpresa.Empresa);
                    cmd.Parameters.AddWithValue("@nGrupoEmpresarial", catEmpresa.GrupoEmpresarial);
                    cmd.Parameters.AddWithValue("@cDescripcion", catEmpresa.Descripcion);
                    cmd.Parameters.AddWithValue("@bActivo", catEmpresa.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catEmpresa.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catEmpresa.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catEmpresa.GrupoEmpresarial = folioSig;
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener las empresas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catEmpresa;
        }

        public async Task<CatEmpresas> ObtenerPorEmpresa(int nEmpresa)
        {
            CatEmpresas linea = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatEmpresas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nEmpresa", nEmpresa);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatEmpresas()
                                {
                                    Empresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    GrupoEmpresarial = ConvertUtils.ToInt32(reader["nGrupoEmpresarial"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    DescripcionGrupoEmpresarial = ConvertUtils.ToString(reader["cDescripcionGrupoEmpresarial"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener las empresas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return linea;
        }

        public async Task<List<CatEmpresas>> Lista()
        {
            List<CatEmpresas> Empresas = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatEmpresas",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nEmpresa", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Empresas.Add(
                                new CatEmpresas()
                                {
                                    Empresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    GrupoEmpresarial = ConvertUtils.ToInt32(reader["nGrupoEmpresarial"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    DescripcionGrupoEmpresarial = ConvertUtils.ToString(reader["cDescripcionGrupoEmpresarial"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener las empresas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return Empresas;
        }

        
    }
}
