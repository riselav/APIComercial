
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
    public class CatImpuestosRepositorio : ICatImpuestosRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ICatalagosSATRepositorio _catalagosSATRepositorio;
        private readonly ILogger<CatImpuestosRepositorio> _logger;

        public CatImpuestosRepositorio(ILogger<CatImpuestosRepositorio> logger,Conexion conexion, ICatalagosSATRepositorio catalagosSATRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _catalagosSATRepositorio = catalagosSATRepositorio;
        }

        public async Task<CatImpuestos> IME_CatImpuestos(CatImpuestos catImpuesto)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CatImpuestos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nImpuesto", catImpuesto.Impuesto);                    
                    cmd.Parameters.AddWithValue("@cDescripcion", catImpuesto.Descripcion);
                    cmd.Parameters.AddWithValue("@nPorcentaje", catImpuesto.Porcentaje);
                    cmd.Parameters.AddWithValue("@nTipo", catImpuesto.Tipo);
                    cmd.Parameters.AddWithValue("@bImpuestoImporte", catImpuesto.ImpuestoImporte);
                    cmd.Parameters.AddWithValue("@bExcento", catImpuesto.Excento);
                    cmd.Parameters.AddWithValue("@cImpuesto", catImpuesto.c_Impuesto);
                    cmd.Parameters.AddWithValue("@cTipoFactor", catImpuesto.TipoFactor);
                    cmd.Parameters.AddWithValue("@nTasaOCuota", catImpuesto.TasaOCuota);
                    cmd.Parameters.AddWithValue("@bActivo", catImpuesto.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catImpuesto.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catImpuesto.Maquina);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catImpuesto.Linea = folioSig;
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

            return catImpuesto;
        }

        public async Task<CatImpuestos> ObtenerPorImpuesto(int nImpuesto)
        {
            CatImpuestos linea = null;
            CatImpuestosSAT impSat = new CatImpuestosSAT();
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatImpuestos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nImpuesto", nImpuesto);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            linea=
                                new CatImpuestos()
                                {
                                    Impuesto = ConvertUtils.ToInt32(reader["nImpuesto"]),
                                    Porcentaje = ConvertUtils.ToDecimal(reader["nPorcentaje"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Tipo = ConvertUtils.ToInt16(reader["nTipo"]),
                                    ImpuestoImporte = ConvertUtils.ToBoolean(reader["bImpuestoImporte"]),
                                    Excento = ConvertUtils.ToBoolean(reader["bExcento"]),
                                    c_Impuesto = ConvertUtils.ToString(reader["c_Impuesto"]),
                                    DescripcionCImpuesto = _catalagosSATRepositorio.ObtenerDescripcionCatImpuestoSATPorC_Impuesto(ConvertUtils.ToString(reader["c_Impuesto"])),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    TipoFactor = ConvertUtils.ToString(reader["cTipoFactor"]),
                                    TasaOCuota = ConvertUtils.ToDecimal(reader["nTasaOCuota"])
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

        public async Task<List<CatImpuestos>> Lista()
        {
            List<CatImpuestos> Impuestos = [];
            CatImpuestosSAT impSat= new CatImpuestosSAT();
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatImpuestos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nImpuesto", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Impuestos.Add(
                                new CatImpuestos()
                                {
                                    Impuesto = ConvertUtils.ToInt32(reader["nImpuesto"]),
                                    Porcentaje = ConvertUtils.ToDecimal(reader["nPorcentaje"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Tipo = ConvertUtils.ToInt16(reader["nTipo"]),
                                    ImpuestoImporte = ConvertUtils.ToBoolean(reader["bImpuestoImporte"]),
                                    Excento = ConvertUtils.ToBoolean(reader["bExcento"]),
                                    c_Impuesto = ConvertUtils.ToString(reader["c_Impuesto"]),
                                    DescripcionCImpuesto = _catalagosSATRepositorio.ObtenerDescripcionCatImpuestoSATPorC_Impuesto(ConvertUtils.ToString(reader["c_Impuesto"])),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    TipoFactor = ConvertUtils.ToString(reader["cTipoFactor"]),
                                    TasaOCuota = ConvertUtils.ToDecimal(reader["nTasaOCuota"])
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

            return Impuestos;
        }

        
    }
}
