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
        private readonly ICatRFCRepositorio _catRFCRepositorio;

        public CatClientesRepositorio(ILogger<CatClientesRepositorio> logger, Conexion conexion, ICatRFCRepositorio catRFCRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _catRFCRepositorio = catRFCRepositorio;
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
                        int TipoPersonaIndex = reader.GetOrdinal("nTipoPersona");
                        int IdRFCIndex = reader.GetOrdinal("nIDRFC");

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
                                    //nTipoPersona = reader.IsDBNull(TipoPersonaIndex) ? 0 : reader.GetInt32(TipoPersonaIndex),
                                    nTipoPersona = reader.IsDBNull(TipoPersonaIndex) ? 0 : Convert.ToInt32(reader.GetByte(TipoPersonaIndex)),

                                    nIDRFC = reader.IsDBNull(IdRFCIndex) ? 0 : reader.GetInt64(IdRFCIndex),

                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),

                                    Estado = ConvertUtils.ToString(reader["cEstado"]),
                                    Municipio = ConvertUtils.ToString(reader["cMunicipio"]),
                                    NombreMunicipio = ConvertUtils.ToString(reader["cNombreMunicipio"]),
                                    Localidad = ConvertUtils.ToString(reader["cLocalidad"]),
                                    NombreLocalidad = ConvertUtils.ToString(reader["cNombreLocalidad"]),
                                    
                                    CatRFC = await _catRFCRepositorio.ObtenerPorRFC(ConvertUtils.ToString(reader["cRFC"]))
                                };

                                if (n_Cliente > 0)
                                {
                                    // Mover al siguiente resultado (tabla contactos)
                                    if (reader.NextResult())
                                    {
                                        catCliente.ContactoCliente = [];
                                        int TipoContactoIndex = reader.GetOrdinal("nTipoContacto");
                                    while (reader.Read())
                                        {

                                        catCliente?.ContactoCliente.Add(new ContactoCliente
                                        {
                                            cliente = Convert.ToInt64(reader["nCliente"]),
                                            contacto = Convert.ToInt32(reader["nContacto"]),
                                            nombre = reader["cNombre"].ToString(),
                                            puesto = reader["cPuesto"].ToString(),
                                            telefono = reader["cTelefono"].ToString(),
                                            celular = reader["cCelular"].ToString(),
                                            correoElectronico = reader["cCorreoElectronico"].ToString(),
                                            tipoContacto = Convert.ToInt32(reader.GetByte(TipoContactoIndex)),
                                            descripcionTipoContacto = reader["cTipoContacto"].ToString()
                                        }
                                         );
                                        }
                                    }
                                }                                

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

        public async Task<CatClientes> IME_Cliente(CatClientes cliente)
        {
            using (var con = _conexion.ObtenerSqlConexion())
            {
                await con.OpenAsync();

                using (var transaction = con.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand()
                        {
                            Connection = con,
                            Transaction = transaction,
                            CommandText = "CAT_IME_Clientes",
                            CommandType = CommandType.StoredProcedure,
                        };
                        cmd.Parameters.AddWithValue("@nFolio", cliente.nCliente);
                        cmd.Parameters.AddWithValue("@cRazonSocial", cliente.CatRFC?.cRazonSocial ?? "");
                        cmd.Parameters.AddWithValue("@cNombreCompleto", cliente.cNombreCompleto);
                        cmd.Parameters.AddWithValue("@nTipoPersona", cliente.nTipoPersona);
                        cmd.Parameters.AddWithValue("@cRFC", cliente.CatRFC?.cRFC ?? "");
                        cmd.Parameters.AddWithValue("@cRegimenFiscal", cliente.CatRFC?.cRegimenFiscal ?? "");
                        cmd.Parameters.AddWithValue("@cCalle", cliente.cCalle);
                        cmd.Parameters.AddWithValue("@cNumExt", cliente.cNumExt);
                        cmd.Parameters.AddWithValue("@cNumInt", cliente.cNumInt);
                        cmd.Parameters.AddWithValue("@cCodigoPostal", cliente.cCodigoPostal);
                        cmd.Parameters.AddWithValue("@cCveColonia", cliente.cColonia);

                        cmd.Parameters.AddWithValue("@cTelefono", cliente.cTelefono);
                        cmd.Parameters.AddWithValue("@cSeniasParticulares", cliente.cSeniasParticulares);
                        cmd.Parameters.AddWithValue("@bActivo", cliente.Activo);
                        cmd.Parameters.AddWithValue("@cUsuario", cliente.Usuario);
                        cmd.Parameters.AddWithValue("@cNombreMaquina ", cliente.Maquina);
                        cmd.Parameters.AddWithValue("@nSucursalRegistro", cliente.nSucursalRegistro);

                        //SqlParameter outputParam = new SqlParameter("@nVenta", SqlDbType.BigInt);
                        //outputParam.Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add(outputParam);

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
                        
                        int folioSig = (int)(returnParameter.Value ?? 0);

                        if (cliente.nCliente== 0) 
                               cliente.nCliente = folioSig;
                        
                        int Renglon = 1;

                        if (cliente.ContactoCliente != null && cliente.ContactoCliente?.Count > 0)
                        {
                            DataTable vdt = _conexion.ObtenerEsquemaTabla("CAT_ClientesContactos");

                            Renglon = 1;
                            foreach (ContactoCliente contacto in cliente.ContactoCliente)
                            {
                                contacto.cliente = folioSig;
                                vdt.Rows.Add(contacto.cliente,
                                    Renglon, 
                                    contacto.nombre,
                                    contacto.puesto,
                                    contacto.telefono,
                                    contacto.celular,
                                    contacto.correoElectronico,
                                    contacto.tipoContacto,
                                    contacto.activo,
                                    contacto.usuario,
                                    contacto.maquina,
                                    "1900-01-01"
                                    );

                                Renglon += 1;
                            }

                            Boolean result = _conexion.InsertarConBulkCopy(con, vdt.TableName, vdt,transaction);
                        }

                        // Todo bien, commit
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                        string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                        int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                        _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                        throw new DataAccessException("Error(rp) al insertar/editar cliente")
                        {
                            Metodo = "IME_Cliente",
                            ErrorMessage = ex.Message,
                            ErrorCode = 1
                        };
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return cliente;
        }
    }
}
