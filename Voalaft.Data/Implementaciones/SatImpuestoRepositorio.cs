
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
    public class SatImpuestoRepositorio : ISatImpuestoRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<SatImpuestoRepositorio> _logger;

        public SatImpuestoRepositorio(ILogger<SatImpuestoRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        
        public async Task<SatImpuesto> ObtenerPorImpuesto(string c_Impuesto)
        {
            SatImpuesto tipoFactor = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_Impuesto",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@c_Impuesto", c_Impuesto);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactor=
                                new SatImpuesto()
                                {
                                    c_Impuesto = ConvertUtils.ToString(reader["c_Impuesto"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"]),
                                    Retencion = ConvertUtils.ToString(reader["Retencion"]),
                                    Traslado = ConvertUtils.ToString(reader["Traslado"]),
                                    Local_o_federal = ConvertUtils.ToString(reader["Local_o_federal"])
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

        public async Task<List<SatImpuesto>> Lista()
        {
            List<SatImpuesto> tipoFactors = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_Impuesto",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@c_Impuesto", string.Empty);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactors.Add(
                                new SatImpuesto()
                                {
                                    c_Impuesto = ConvertUtils.ToString(reader["c_Impuesto"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"]),
                                    Retencion = ConvertUtils.ToString(reader["Retencion"]),
                                    Traslado = ConvertUtils.ToString(reader["Traslado"]),
                                    Local_o_federal = ConvertUtils.ToString(reader["Local_o_federal"])
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
