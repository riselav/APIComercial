using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class SatCatUsoCFDIRepositorio: ISatCatUsoCFDIRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<SatCatUsoCFDIRepositorio> _logger;

        public SatCatUsoCFDIRepositorio(ILogger<SatCatUsoCFDIRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<SatCatUsoCFDI> ObtenerPorClave(string c_UsoCFDI)
        {
            SatCatUsoCFDI satCatUsoCFDI = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_UsoCFDI_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cUsoCFDI", c_UsoCFDI);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int RegimenFiscalReceptorIndex = reader.GetOrdinal("RegimenFiscalReceptor");

                        while (await reader.ReadAsync())
                        {
                            satCatUsoCFDI =
                                new SatCatUsoCFDI()
                                {
                                    UsoCFDI = ConvertUtils.ToString(reader["UsoCFDI"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"]),
                                    AplicaPersonaFisica = ConvertUtils.ToBoolean(reader["AplicaPersonaFisica"]),
                                    AplicaPersonaMoral = ConvertUtils.ToBoolean(reader["AplicaPersonaMoral"]),
                                    FechaInicioVigencia = ConvertUtils.ToDateTime(reader["FechaInicioVigencia"]),
                                    RegimenFiscalReceptor = reader.IsDBNull(RegimenFiscalReceptorIndex) ? "" : reader.GetString(RegimenFiscalReceptorIndex)
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
                throw new DataAccessException("Error(rp) No se pudo obtener Uso CFDI")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return satCatUsoCFDI;
        }
        
        public async Task<List<SatCatUsoCFDI>> Lista()
        {
            List<SatCatUsoCFDI> satCatUsoCFDI = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_UsoCFDI_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cUsoCFDI", string.Empty);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int RegimenFiscalReceptorIndex = reader.GetOrdinal("RegimenFiscalReceptor");

                        while (await reader.ReadAsync())
                        {
                            satCatUsoCFDI.Add(
                                new SatCatUsoCFDI()
                                {
                                    UsoCFDI = ConvertUtils.ToString(reader["UsoCFDI"]),
                                    Descripcion = ConvertUtils.ToString(reader["Descripcion"]),
                                    AplicaPersonaFisica = ConvertUtils.ToBoolean(reader["AplicaPersonaFisica"]),
                                    AplicaPersonaMoral = ConvertUtils.ToBoolean(reader["AplicaPersonaMoral"]),
                                    FechaInicioVigencia = ConvertUtils.ToDateTime(reader["FechaInicioVigencia"]),
                                    RegimenFiscalReceptor = reader.IsDBNull(RegimenFiscalReceptorIndex) ? "" : reader.GetString(RegimenFiscalReceptorIndex)
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
                throw new DataAccessException("Error(rp) No se pudo obtener uso vfdi")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return satCatUsoCFDI;
        }
    }
}
