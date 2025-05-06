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
    public class CatCorreoContactoRFCRepositorio: ICatCorreoContactoRFCRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatCorreoContactoRFCRepositorio> _logger;

        public CatCorreoContactoRFCRepositorio(ILogger<CatCorreoContactoRFCRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<List<CatCorreoContactoRFC>> ObtenerPorId(long n_IDRFCRegimenFiscal)
        {
            List<CatCorreoContactoRFC> catCorreoContactoRFC = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CorreosContactoRFC_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nIDRFC", n_IDRFCRegimenFiscal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            catCorreoContactoRFC.Add(
                                new CatCorreoContactoRFC()
                                {
                                    IDRFC = ConvertUtils.ToInt64(reader["nIDRFC"]),
                                    Folio = ConvertUtils.ToInt32(reader["nFolio"]),
                                    CorreoElectronico = ConvertUtils.ToString(reader["cCorreoElectronico"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario_Registra"]),
                                    Maquina = ConvertUtils.ToString(reader["cMaquina_Registra"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los correos de contacto rfc")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catCorreoContactoRFC;
        }
    }
}
