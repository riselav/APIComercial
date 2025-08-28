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
using Voalaft.Data.Entidades.Tableros;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatSucursalesRepositorio : ICatSucursalesRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatSucursalesRepositorio> _logger;
        public CatSucursalesRepositorio(ILogger<CatSucursalesRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatSucursales> IME_CatSucursales(CatSucursales catSucursal)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_Sucursales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", catSucursal.nSucursal);
                    cmd.Parameters.AddWithValue("@cDescripcion", catSucursal.cDescripcion);
                    cmd.Parameters.AddWithValue("@nEmpresa", catSucursal.nEmpresa);
                    cmd.Parameters.AddWithValue("@nPlaza", catSucursal.nPlaza);
                    cmd.Parameters.AddWithValue("@nRegion", catSucursal.nRegion);
                    cmd.Parameters.AddWithValue("@cEstado", catSucursal.cEstado);
                    cmd.Parameters.AddWithValue("@cLocalidad", catSucursal.cLocalidad);
                    cmd.Parameters.AddWithValue("@cMunicipio", catSucursal.cMunicipio);
                    cmd.Parameters.AddWithValue("@cCodigoPostal", catSucursal.cCodigoPostal);
                    cmd.Parameters.AddWithValue("@cColonia", catSucursal.cColonia);
                    cmd.Parameters.AddWithValue("@nZona", catSucursal.nZona);
                    cmd.Parameters.AddWithValue("@cDomicilio", catSucursal.cDomicilio);
                    cmd.Parameters.AddWithValue("@cTelefono1", catSucursal.cTelefono1);
                    cmd.Parameters.AddWithValue("@cTelefono2", catSucursal.cTelefono2);
                    cmd.Parameters.AddWithValue("@bActivo", catSucursal.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catSucursal.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catSucursal.Maquina);
                    //await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catLinea.Linea = folioSig;

                    // Agrega el parámetro de retorno
                    var returnParameter = new SqlParameter
                    {
                        ParameterName = "@RETURN_VALUE",
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParameter);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;

                    //long valorOutput = (long)cmd.Parameters["@nVenta"].Value;

                    bool nvo = false;
                    int folioSig = (int)(returnParameter.Value ?? 0);

                    if (catSucursal.nSucursal == 0)
                    {
                        nvo = true;
                        catSucursal.nSucursal = folioSig;
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (Sucursales {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "IME_CatSucursales",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catSucursal;
        }


        public async Task<CatSucursales> ObtenerSucursal(int nSucursal)
        {
            CatSucursales objSucursal = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Sucursales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", objSucursal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            objSucursal =
                                new CatSucursales()
                                {
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nEmpresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    NombreEmpresa = ConvertUtils.ToString(reader["NombreEmpresa"]),

                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    NombrePlaza = ConvertUtils.ToString(reader["NombrePlaza"]),

                                    nRegion = ConvertUtils.ToInt32(reader["nRegion"]),
                                    cEstado = ConvertUtils.ToString(reader["cEstado"]),
                                    cLocalidad = ConvertUtils.ToString(reader["cLocalidad"]),
                                    cMunicipio = ConvertUtils.ToString(reader["cMunicipio"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    nZona = ConvertUtils.ToInt32(reader["nZona"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cTelefono1 = ConvertUtils.ToString(reader["cTelefono1"]),
                                    cTelefono2 = ConvertUtils.ToString(reader["cTelefono2"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return objSucursal;
        }


        public async Task<List<CatSucursales>> Lista()
        {
            List<CatSucursales> Sucursales = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Sucursales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Sucursales.Add(
                                new CatSucursales()
                                {
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nEmpresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    NombreEmpresa = ConvertUtils.ToString(reader["NombreEmpresa"]),

                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    NombrePlaza = ConvertUtils.ToString(reader["NombrePlaza"]),

                                    nRegion = ConvertUtils.ToInt32(reader["nRegion"]),
                                    cEstado = ConvertUtils.ToString(reader["cEstado"]),
                                    cLocalidad = ConvertUtils.ToString(reader["cLocalidad"]),
                                    cMunicipio = ConvertUtils.ToString(reader["cMunicipio"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),

                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    NombreColonia = ConvertUtils.ToString(reader["NombreColonia"]),

                                    nZona = ConvertUtils.ToInt32(reader["nZona"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cTelefono1 = ConvertUtils.ToString(reader["cTelefono1"]),
                                    cTelefono2 = ConvertUtils.ToString(reader["cTelefono2"]),
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

                _logger.LogError($"Error en {className}.{methodName} (Sucursales {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener las sucursales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return Sucursales;
        }

        // Normaliza la lista a 9 elementos (string), aplica Trim, null->"", y máximo 50 chars
        private static string[] PadTo9(List<string>? input)
        {
            var list = (input ?? new List<string>())
                .Select(s => (s ?? string.Empty).Trim())
                .Take(9)
                .ToList();

            while (list.Count < 9) list.Add(string.Empty);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Length > 50) list[i] = list[i].Substring(0, 50);
            }

            return list.ToArray();
        }


        public async Task<CatSucursales> IME_ConfiguracionTicketSucursal(CatSucursales catSucursal)
        {
            if (catSucursal is null) throw new ArgumentNullException(nameof(catSucursal));

            using var con = _conexion.ObtenerSqlConexion();
            await con.OpenAsync();

            // Transacción async
            using var transaction = (SqlTransaction)await con.BeginTransactionAsync();

            try
            {
                var e = PadTo9(catSucursal.EncabezadoTicket);
                var p = PadTo9(catSucursal.PieTicket);

                // ----- Encabezados -----
                using (var cmd = new SqlCommand("RST_IME_CAT_EncabezadosSucursal", con, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@nSucursal", SqlDbType.Int).Value = catSucursal.nSucursal;
                    for (int i = 0; i < 9; i++)
                    {
                        cmd.Parameters.Add($"@cEncabezado{i + 1}", SqlDbType.VarChar, 50).Value = e[i];
                    }

                    await cmd.ExecuteNonQueryAsync();
                }

                // ----- Pie de página -----
                using (var cmd = new SqlCommand("RST_IME_CAT_PiePaginaSucursal", con, transaction) // ajusta el nombre si difiere
                {
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@nSucursal", SqlDbType.Int).Value = catSucursal.nSucursal;
                    for (int i = 0; i < 9; i++)
                    {
                        cmd.Parameters.Add($"@cPiePagina{i + 1}", SqlDbType.VarChar, 50).Value = p[i];
                    }

                    await cmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();

                return catSucursal;
            }
            catch (Exception ex)
            {
                try { await transaction.RollbackAsync(); } catch { /* opcional: log de rollback */ }

                _logger.LogError(ex, "Error en IME_ConfiguracionTicketSucursal (Sucursal {Sucursal})", catSucursal?.nSucursal);

                throw new DataAccessException("Error(rp) al guardar configuración de ticket de sucursal")
                {
                    Metodo = "IME_ConfiguracionTicketSucursal",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            finally
            {
                await con.CloseAsync();
            }
        }

        public async Task<CatConfiguracionTicketSucursal> ObtenerConfiguracionTicketSucursal(int nSucursal)
        {
            CatConfiguracionTicketSucursal configuracion = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_ConfiguracionTicketSucursal",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", nSucursal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        configuracion = new CatConfiguracionTicketSucursal
                        {
                            Sucursal = nSucursal,
                            EncabezadoTicket = new List<string>(),
                            PieTicket = new List<string>()
                        };

                        // ---- Result set 1: Encabezados ----
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            configuracion.EncabezadoTicket = Read9(reader, "cEncabezado");
                        }
                        else
                        {
                            configuracion.EncabezadoTicket = Pad9(null);
                        }

                        // ---- Result set 2: Pie ----
                        if (await reader.NextResultAsync())
                        {
                            if (reader.HasRows && await reader.ReadAsync())
                                configuracion.PieTicket = Read9(reader, "cPieTicket");
                            else
                                configuracion.PieTicket = Pad9(null);
                        }
                        else
                        {
                            configuracion.PieTicket = Pad9(null);
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
                throw new DataAccessException("Error(rp) No se pudo obtener la configuracion de ticket de sucursal")
                {
                    Metodo = "ObtenerConfiguracionTicketSucursal",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return configuracion;
        }

        // Lee 9 columnas con prefijo base (p.ej. "cEncabezado" => cEncabezado1..9)
        private static List<string> Read9(SqlDataReader rd, string baseName)
        {
            var vals = new List<string>(capacity: 9);
            for (int i = 1; i <= 9; i++)
            {
                var colName = $"{baseName}{i}";
                string val = TryGetString(rd, colName);
                vals.Add(Norm50(val));
            }
            return Pad9(vals);
        }

        // Obtiene string seguro por nombre de columna (si no existe o es null → "")
        private static string TryGetString(SqlDataReader rd, string colName)
        {
            try
            {
                int ord = rd.GetOrdinal(colName);          // throws si no existe
                if (rd.IsDBNull(ord)) return string.Empty; // null -> ""
                return rd.GetString(ord);
            }
            catch (IndexOutOfRangeException)
            {
                // Columna no existe en el result set → ""
                return string.Empty;
            }
        }

        // Normaliza: Trim y recorta a 50
        private static string Norm50(string? s) =>
            (s ?? string.Empty).Trim() is var t
                ? (t.Length > 50 ? t[..50] : t)
                : string.Empty;

        // Asegura exactamente 9 elementos (rellena con "")
        private static List<string> Pad9(IEnumerable<string>? input)
        {
            var list = (input ?? Enumerable.Empty<string>()).Take(9).ToList();
            while (list.Count < 9) list.Add(string.Empty);
            return list;
        }
    }
}
