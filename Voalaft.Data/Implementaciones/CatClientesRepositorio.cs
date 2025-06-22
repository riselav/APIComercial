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
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatClientesRepositorio:ICatClientesRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatClientesRepositorio> _logger;

        public CatClientesRepositorio(ILogger<CatClientesRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatClientes> ObtenerPorId(long n_Cliente)
        {
            CatClientes catCliente = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Clientes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", n_Cliente);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            catCliente =
                                new CatClientes()
                                {
                                    nCliente = ConvertUtils.ToInt64(reader["nCliente"]),
                                    cNombreCompleto = ConvertUtils.ToString(reader["cCliente"]),
                                    cCalle = ConvertUtils.ToString(reader["cCalle"]),
                                    cNumExt = ConvertUtils.ToString(reader["cNumExt"]),
                                    cNumInt = ConvertUtils.ToString(reader["cNumInt"]),
                                    cColonia= ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal= ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cTelefono = ConvertUtils.ToString(reader["cTelefono"]),
                                    cSeniasParticulares = ConvertUtils.ToString(reader["cSeniasParticulares"]),
                                    nSucursalRegistro = reader.IsDBNull(SucursalRegistroIndex) ? 0 : reader.GetInt32(SucursalRegistroIndex),
                                    Activo= ConvertUtils.ToBoolean(reader["bActivo"]),

                                    Estado = ConvertUtils.ToString(reader["cEstado"]),
                                    Municipio = ConvertUtils.ToString(reader["cMunicipio"]),
                                    NombreMunicipio = ConvertUtils.ToString(reader["cNombreMunicipio"]),
                                    Localidad = ConvertUtils.ToString(reader["cLocalidad"]),
                                    NombreLocalidad = ConvertUtils.ToString(reader["cNombreLocalidad"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener cliente")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catCliente;
        }

        public async Task<List<CatClientes>> Lista()
        {
            List<CatClientes> clientes = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Clientes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int SucursalRegistroIndex = reader.GetOrdinal("nSucursalRegistro");

                        while (await reader.ReadAsync())
                        {
                            clientes.Add(
                                new CatClientes()
                                {
                                    nCliente = ConvertUtils.ToInt64(reader["nCliente"]),
                                    cNombreCompleto = ConvertUtils.ToString(reader["cCliente"]),
                                    cCalle = ConvertUtils.ToString(reader["cCalle"]),
                                    cNumExt = ConvertUtils.ToString(reader["cNumExt"]),
                                    cNumInt = ConvertUtils.ToString(reader["cNumInt"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cTelefono = ConvertUtils.ToString(reader["cTelefono"]),
                                    cSeniasParticulares = ConvertUtils.ToString(reader["cSeniasParticulares"]),
                                    nSucursalRegistro = reader.IsDBNull(SucursalRegistroIndex) ? 0 : reader.GetInt32(SucursalRegistroIndex),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),

                                    Estado = ConvertUtils.ToString(reader["cEstado"]),
                                    Municipio = ConvertUtils.ToString(reader["cMunicipio"]),
                                    NombreMunicipio = ConvertUtils.ToString(reader["cNombreMunicipio"]),
                                    Localidad = ConvertUtils.ToString(reader["cLocalidad"]),
                                    NombreLocalidad = ConvertUtils.ToString(reader["cNombreLocalidad"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de los clientes")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return clientes;
        }

        public async Task<List<Cliente>> ConsultaClientes(ParametrosConsultaClientes paramClientes)
        {
            List<Cliente> clientes = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_Tablero_Clientes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCliente", paramClientes.RazonSocial);
                    cmd.Parameters.AddWithValue("@cRFC", paramClientes.RFC);
                    cmd.Parameters.AddWithValue("@nRegimen", paramClientes.RegimenFiscal);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int CiudadIndex = reader.GetOrdinal("ciudad");

                        int RegimenIndex = reader.GetOrdinal("regimenFiscal");
                        int RazonSocialIndex = reader.GetOrdinal("razonSocial");
                        int RFCIndex = reader.GetOrdinal("rfc");
                
                        while (await reader.ReadAsync())
                        {
                            clientes.Add(
                                new Cliente()
                                {
                                    codigoCliente = ConvertUtils.ToInt64(reader["codigoCliente"]),
                                    nombreComercial = ConvertUtils.ToString(reader["nombreComercial"]),

                                    calle = ConvertUtils.ToString(reader["calle"]),
                                    colonia = ConvertUtils.ToString(reader["colonia"]),
                                    ciudad = reader.IsDBNull(CiudadIndex) ? "" : reader.GetString(CiudadIndex),
                                    estado = ConvertUtils.ToString(reader["estado"]),

                                    regimenFiscal = reader.IsDBNull(RegimenIndex) ? "" : reader.GetString(RegimenIndex),
                                    razonSocial = reader.IsDBNull(RazonSocialIndex) ? "" : reader.GetString(RazonSocialIndex),
                                    rfc = reader.IsDBNull(RFCIndex) ? "" : reader.GetString(RFCIndex),

                                    activo= ConvertUtils.ToBoolean(reader["activo"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener la lista de los clientes de tablero")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return clientes;
        }
    }
}
