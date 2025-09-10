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
    public class CatMotivosRepositorio : ICatMotivosRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatMotivosRepositorio> _logger;

        public CatMotivosRepositorio(ILogger<CatMotivosRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista de motivos por su tipo.
        /// </summary>
        /// <param name="nTipoMotivo">El tipo de motivo por el cual filtrar.</param>
        public async Task<List<CatMotivos>> ObtenerPorTipoMotivo(int nTipoMotivo)
        {
            List<CatMotivos> motivos = new List<CatMotivos>();
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SELECT nMotivo, cDescripcion, nTipoMotivo, bActivo, cMaquina_Registra, cUsuario_Registra, dFecha_Registra, cMaquina_Modifica, cUsuario_Modifica, dFecha_Modifica, cMaquina_Cancela, cUsuario_Cancela, dFecha_Cancela FROM [dbo].[CAT_Motivos] WHERE nTipoMotivo = @nTipoMotivo AND bActivo = 1",
                        CommandType = CommandType.Text,
                    };
                    cmd.Parameters.AddWithValue("@nTipoMotivo", nTipoMotivo);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            motivos.Add(MapReaderToCatMotivos(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener motivos por tipo de motivo: {nTipoMotivo}", nTipoMotivo);
                throw new DataAccessException("Error(rp) No se pudieron obtener los motivos por tipo.")
                {
                    Metodo = "ObtenerPorTipoMotivo",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return motivos;
        }

        /// <summary>
        /// Obtiene una lista de todos los motivos activos.
        /// </summary>
        public async Task<List<CatMotivos>> Lista()
        {
            List<CatMotivos> motivos = new List<CatMotivos>();
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    await con.OpenAsync();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SELECT nMotivo, cDescripcion, nTipoMotivo, bActivo, cMaquina_Registra, cUsuario_Registra, dFecha_Registra, cMaquina_Modifica, cUsuario_Modifica, dFecha_Modifica, cMaquina_Cancela, cUsuario_Cancela, dFecha_Cancela FROM [dbo].[CAT_Motivos] WHERE bActivo = 1",
                        CommandType = CommandType.Text,
                    };
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            motivos.Add(MapReaderToCatMotivos(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de motivos.");
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de motivos.")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return motivos;
        }

        /// <summary>
        /// Mapea los datos de un SqlDataReader a un objeto CatMotivos.
        /// </summary>
        private CatMotivos MapReaderToCatMotivos(SqlDataReader reader)
        {
            return new CatMotivos
            {
                nMotivo = reader.GetInt32(reader.GetOrdinal("nMotivo")),
                cDescripcion = reader.GetString(reader.GetOrdinal("cDescripcion")),
                nTipoMotivo = reader.GetInt32(reader.GetOrdinal("nTipoMotivo")),
                bActivo = reader.GetBoolean(reader.GetOrdinal("bActivo")),
                cMaquina_Registra = reader.IsDBNull(reader.GetOrdinal("cMaquina_Registra")) ? null : reader.GetString(reader.GetOrdinal("cMaquina_Registra")),
                cUsuario_Registra = reader.IsDBNull(reader.GetOrdinal("cUsuario_Registra")) ? null : reader.GetString(reader.GetOrdinal("cUsuario_Registra")),
                dFecha_Registra = reader.IsDBNull(reader.GetOrdinal("dFecha_Registra")) ? null : reader.GetDateTime(reader.GetOrdinal("dFecha_Registra")),
                cMaquina_Modifica = reader.IsDBNull(reader.GetOrdinal("cMaquina_Modifica")) ? null : reader.GetString(reader.GetOrdinal("cMaquina_Modifica")),
                cUsuario_Modifica = reader.IsDBNull(reader.GetOrdinal("cUsuario_Modifica")) ? null : reader.GetString(reader.GetOrdinal("cUsuario_Modifica")),
                dFecha_Modifica = reader.IsDBNull(reader.GetOrdinal("dFecha_Modifica")) ? null : reader.GetDateTime(reader.GetOrdinal("dFecha_Modifica")),
                cMaquina_Cancela = reader.IsDBNull(reader.GetOrdinal("cMaquina_Cancela")) ? null : reader.GetString(reader.GetOrdinal("cMaquina_Cancela")),
                cUsuario_Cancela = reader.IsDBNull(reader.GetOrdinal("cUsuario_Cancela")) ? null : reader.GetString(reader.GetOrdinal("cUsuario_Cancela")),
                dFecha_Cancela = reader.IsDBNull(reader.GetOrdinal("dFecha_Cancela")) ? null : reader.GetDateTime(reader.GetOrdinal("dFecha_Cancela"))
            };
        }
    }
}