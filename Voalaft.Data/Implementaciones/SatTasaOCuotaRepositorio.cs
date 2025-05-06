
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
    public class SatTasaOCuotaRepositorio : ISatTasaOCuotaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<SatTasaOCuotaRepositorio> _logger;

        public SatTasaOCuotaRepositorio(ILogger<SatTasaOCuotaRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        
        public async Task<SatTasaOCuota> ObtenerPorTasaOCuota(string cValor)
        {
            SatTasaOCuota tipoFactor = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_TasaOCuota",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cValor", cValor);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactor=
                                new SatTasaOCuota()
                                {
                                    cValor = ConvertUtils.ToString(reader["cValor"]),
                                    Impuesto = ConvertUtils.ToString(reader["Impuesto"]),
                                    Factor = ConvertUtils.ToString(reader["Factor"]),
                                    Traslado = ConvertUtils.ToString(reader["Traslado"]),
                                    Retencion = ConvertUtils.ToString(reader["Retencion"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener tasa o cuota")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return tipoFactor;
        }

        public async Task<List<SatTasaOCuota>> Lista()
        {
            List<SatTasaOCuota> tipoFactors = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_TasaOCuota",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cValor", string.Empty);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tipoFactors.Add(
                                new SatTasaOCuota()
                                {
                                    cValor = ConvertUtils.ToString(reader["cValor"]),
                                    Impuesto = ConvertUtils.ToString(reader["Impuesto"]),
                                    Factor = ConvertUtils.ToString(reader["Factor"]),
                                    Traslado = ConvertUtils.ToString(reader["Traslado"]),
                                    Retencion = ConvertUtils.ToString(reader["Retencion"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los tasa o cuota")
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
