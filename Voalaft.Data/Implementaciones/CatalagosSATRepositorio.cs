
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
    public class CatalagosSATRepositorio : ICatalagosSATRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatalagosSATRepositorio> _logger;

        public CatalagosSATRepositorio(ILogger<CatalagosSATRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }
  
        public async Task<List<CatUnidadesSat>> ListaCatUnidades()
        {
            List<CatUnidadesSat> catalago = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidadesSat",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cClave", "");
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catalago.Add(
                                new CatUnidadesSat()
                                {
                                    Id = ConvertUtils.ToInt32(reader["nId"]),
                                    Tipo = ConvertUtils.ToString(reader["cTipo"]),
                                    Clave = ConvertUtils.ToString(reader["cClave"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener las unidades SAT")
                {
                    Metodo = "ListaCatUnidades",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catalago;
        }

        public async Task<CatUnidadesSat> ObtenerUnidadesPorClave(string cClave)
        {
            CatUnidadesSat catalago = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatUnidadesSat",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cClave", cClave);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catalago =
                                new CatUnidadesSat()
                                {
                                    Id = ConvertUtils.ToInt32(reader["nId"]),
                                    Tipo = ConvertUtils.ToString(reader["cTipo"]),
                                    Clave = ConvertUtils.ToString(reader["cClave"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener las unidades SAT")
                {
                    Metodo = "ObtenerUnidadesPorClave",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catalago;
        }

        public List<CatImpuestosSAT> ObtenerListaCatImpuestosSAT()
        {
            List<CatImpuestosSAT> resultado = new List<CatImpuestosSAT>();
            resultado.Add(
                new CatImpuestosSAT()
                {
                    c_impuesto = "001",
                    Descripcion = "ISR",
                    EsRetencion = true,
                    EsTraslado = false
                }
                );
            resultado.Add(
                new CatImpuestosSAT()
                {
                    c_impuesto = "002",
                    Descripcion = "IVA",
                    EsRetencion = true,
                    EsTraslado = true
                }
                );
            resultado.Add(
                new CatImpuestosSAT()
                {
                    c_impuesto = "003",
                    Descripcion = "IEPS",
                    EsRetencion = true,
                    EsTraslado = true
                }
                );
            return resultado;
        }

        public  CatImpuestosSAT ObtenerCatImpuestoSATPorC_Impuesto(string c_impuesto)
        {
            return  ObtenerListaCatImpuestosSAT().FirstOrDefault(impuesto => impuesto.c_impuesto == c_impuesto);
        }

        public string ObtenerDescripcionCatImpuestoSATPorC_Impuesto(string c_impuesto)
        {
            CatImpuestosSAT imp = ObtenerListaCatImpuestosSAT().FirstOrDefault(impuesto => impuesto.c_impuesto == c_impuesto);
            if (imp != null)
            {
                return imp.Descripcion;
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
