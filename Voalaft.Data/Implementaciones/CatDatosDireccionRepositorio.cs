
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
    public class CatDatosDireccionRepositorio : ICatDatosDireccionRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatDatosDireccionRepositorio> _logger;

        public CatDatosDireccionRepositorio(ILogger<CatDatosDireccionRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        
        public async Task<List<CatColonias>> ObtenerColoniasPorCP(string cCodigoPostal)
        {
            List<CatColonias> cat = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Colonias_CP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cCodigoPostal", cCodigoPostal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cat.Add(
                                new CatColonias()
                                {
                                    Colonia = ConvertUtils.ToString(reader["cColonia"]),
                                    CodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    NombreAsentamiento = ConvertUtils.ToString(reader["cNombreAsentamiento"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener las colonias")
                {
                    Metodo = "ObtenerPorCP",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cat;
        }

        public async Task<List<CatCodigosPostales>> ListaCodigosPostales()
        {
            List<CatCodigosPostales> cat = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CodigosPostales",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cCodigoPostal", "");
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            cat.Add(
                                new CatCodigosPostales()
                                {
                                    CodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    Estado = ConvertUtils.ToString(reader["cEstado"]),
                                    Localidad = ConvertUtils.ToString(reader["cLocalidad"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los CPs")
                {
                    Metodo = "CatCodigosPostales",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cat;
        }

        public async Task<DireccionPorCodigoPostal> DireccionPorCodigoPostal(string cCodigoPostal)
        {
            DireccionPorCodigoPostal cat = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_DireccionPorCodigoPostal",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@cCodigoPostal", cCodigoPostal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Primer resultado: datos generales
                        if (reader.Read())
                        {
                            cat = new DireccionPorCodigoPostal
                            {
                                Pais = new Entidad
                                {
                                    Id = Convert.ToString(reader["cPais"]),
                                    Nombre = reader["cNombrePais"].ToString()
                                },
                                Estado = new Entidad
                                {
                                    Id = Convert.ToString(reader["cEstado"]),
                                    Nombre = reader["cNombreEstado"].ToString()
                                },
                                Municipio = new Entidad
                                {
                                    Id = Convert.ToString(reader["cMunicipio"]),
                                    Nombre = reader["cNombreMunicipio"].ToString()
                                },
                                Ciudad = new Entidad
                                {
                                    Id = Convert.ToString(reader["cCiudad"]),
                                    Nombre = reader["cNombreCiudad"].ToString()
                                }
                            };
                        }

                        // Mover al siguiente resultado (colonias)
                        if (reader.NextResult())
                        {
                            cat.Colonias = [];
                            while (reader.Read())
                            {
                                
                                cat?.Colonias.Add(new Entidad
                                {
                                    Id = Convert.ToString(reader["cColonia"]),
                                    Nombre = reader["cNombreColonia"].ToString()
                                });
                            }
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
                throw new DataAccessException("Error(rp) No se pudo obtener los datos de direccion de un CP")
                {
                    Metodo = "DireccionPorCodigoPostal",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return cat;
        }


    }
}
