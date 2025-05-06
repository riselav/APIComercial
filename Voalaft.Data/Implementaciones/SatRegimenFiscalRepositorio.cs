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
    public class SatRegimenFiscalRepositorio:ISatRegimenFiscalRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<SatRegimenFiscalRepositorio> _logger;

        public SatRegimenFiscalRepositorio(ILogger<SatRegimenFiscalRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<SatRegimenFiscal> ObtenerPorId(long n_RegimenFiscal)
        {
            SatRegimenFiscal regimenFiscal = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_RegimenFiscal",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nRegimenFiscal", n_RegimenFiscal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            regimenFiscal =
                                new SatRegimenFiscal()
                                {
                                    nIdRegimenFiscal = ConvertUtils.ToInt32(reader["nRegimenFiscal"]),
                                    RegimenFiscalDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    bFisica = ConvertUtils.ToBoolean(reader["bFisica"]),
                                    bMoral = ConvertUtils.ToBoolean(reader["bMoral"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener Regimen Fiscal")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return regimenFiscal;
        }

        public async Task<List<SatRegimenFiscal>> Lista()
        {
            List<SatRegimenFiscal> regimenFiscal = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SAT_CON_RegimenFiscal",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nRegimenFiscal", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            regimenFiscal.Add(
                                new SatRegimenFiscal()
                                {
                                    nIdRegimenFiscal = ConvertUtils.ToInt32(reader["nRegimenFiscal"]),
                                    RegimenFiscalDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    bFisica = ConvertUtils.ToBoolean(reader["bFisica"]),
                                    bMoral = ConvertUtils.ToBoolean(reader["bMoral"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los Regimen Fiscal")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return regimenFiscal;
        }
    }
}