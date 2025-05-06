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
    public class CatRFCRepositorio : ICatRFCRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatRFCRepositorio> _logger;

        public CatRFCRepositorio(ILogger<CatRFCRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatRFC> ObtenerPorRFC(string c_RFC)
        {
            CatRFC catRFC = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_RFC_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cRFC", c_RFC);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        while (await reader.ReadAsync())
                        {
                            catRFC =
                            new CatRFC()
                            {
                                nIDRFC = ConvertUtils.ToInt32(reader["nIDRFC"]),
                                cRazonSocial = ConvertUtils.ToString(reader["cRazonSocial"]),
                                cRFC = ConvertUtils.ToString(reader["cRFC"]),
                                cCP = ConvertUtils.ToString(reader["cCP"]),
                                cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                cUso_CFDI = ConvertUtils.ToString(reader["cUso_CFDI"]),
                                cRegimenFiscal = ConvertUtils.ToString(reader["cRegimenFiscal"]),
                                bActivo = ConvertUtils.ToBoolean(reader["bActivo"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener Cat RFC")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catRFC;
        }

        public async Task<List<CatRFC>> Lista()
        {
            List<CatRFC> catRFC = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_RFC_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cRFC", string.Empty);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        
                        while (await reader.ReadAsync())
                        {
                            catRFC.Add(
                                new CatRFC()
                                {
                                    nIDRFC = ConvertUtils.ToInt32(reader["nIDRFC"]),
                                    cRazonSocial = ConvertUtils.ToString(reader["cRazonSocial"]),
                                    cRFC = ConvertUtils.ToString(reader["cRFC"]),
                                    cCP = ConvertUtils.ToString(reader["cCP"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cUso_CFDI = ConvertUtils.ToString(reader["cUso_CFDI"]),
                                    cRegimenFiscal = ConvertUtils.ToString(reader["cRegimenFiscal"]),
                                    bActivo = ConvertUtils.ToBoolean(reader["bActivo"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener lista cat rfc")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catRFC;
        }
    }
}
