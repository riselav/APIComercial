using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SQLConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatClientesRepositorio : ICatClientesRepositorio
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
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
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

                                // Mover al siguiente resultado (tabla contactos)
                                if (reader.NextResult())
                                {
                                    catCliente.CorreosCliente = [];

                                    while (reader.Read())
                                    {
                                        catCliente?.CorreosCliente.Add(new CatCorreoContactoRFC
                                        {
                                            IDRFC = Convert.ToInt64(reader["IDRFC"]),
                                            Folio = Convert.ToInt32(reader["Folio"]),
                                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                            Activo = Convert.ToBoolean(reader["Activo"]),
                                            Usuario = reader["Usuario"].ToString(),
                                            Maquina = reader["Maquina"].ToString()
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

                                    activo = ConvertUtils.ToBoolean(reader["activo"]),
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
            CatClientes cte = cliente;

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
                        cmd.Parameters.AddWithValue("@cUso_CFDI", cliente.CatRFC?.cUso_CFDI ?? "");

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

                        bool nvo=false;
                        int folioSig = (int)(returnParameter.Value ?? 0);

                        if (cliente.nCliente == 0)
                        {
                            nvo = true;
                            cliente.nFolio = folioSig;
                            cliente.nCliente = cliente.GenerateID();
                        }
                        int Renglon = 1;

                        if (cliente.ContactoCliente != null && cliente.ContactoCliente?.Count > 0)
                        {
                            bool respuesta = await EliminarContactosCliente(cliente.nCliente, con, transaction);

                            if (respuesta)
                            {
                                DataTable vdt = _conexion.ObtenerEsquemaTabla("CAT_ClientesContactos");

                                Renglon = 1;
                                foreach (ContactoCliente contacto in cliente.ContactoCliente)
                                {
                                    if (folioSig == 1)
                                    {
                                        contacto.cliente = folioSig;
                                    }
                                    else
                                    {
                                        contacto.cliente = cliente.nCliente;
                                    }

                                    vdt.Rows.Add(contacto.cliente,
                                        Renglon,
                                        contacto.nombre,
                                        contacto.puesto,
                                        contacto.telefono,
                                        contacto.celular,
                                        contacto.correoElectronico,
                                        contacto.tipoContacto,
                                        true,//contacto.activo,
                                        cliente.Usuario,
                                        cliente.Maquina,
                                        DateTime.Now
                                        );

                                    Renglon += 1;
                                }

                                Boolean result = _conexion.InsertarConBulkCopy(con, vdt.TableName, vdt, transaction);
                                if (!result)
                                {
                                    transaction.Rollback();
                                    cte = null;
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                cte = null;
                            }
                            
                        }

                        long idRFC = await ObtenIdRFC_Cliente(cliente.nCliente);
                        
                        cliente.nIDRFC = idRFC;

                        if (cliente.CorreosCliente != null && cliente.CorreosCliente?.Count > 0)
                        {
                            bool respuesta = await EliminarCorreosCliente(idRFC, con, transaction);

                            if(respuesta)
                            {
                                DataTable vdt = _conexion.ObtenerEsquemaTabla("CAT_CorreosContactoRFC");

                                Renglon = 1;

                               
                                foreach (CatCorreoContactoRFC correo in cliente.CorreosCliente)
                                {
                                    //cliente.CatRFC?.cUso_CFDI ?? ""
                                    correo.IDRFC = idRFC;

                                    vdt.Rows.Add(correo.IDRFC,
                                        Renglon,
                                        correo.CorreoElectronico,
                                        true, //correo.Activo ?? true,
                                        correo.Usuario ?? cliente.Usuario,
                                        correo.Maquina ?? cliente.Maquina,
                                        DateTime.Now
                                        );

                                    Renglon += 1;
                                }
                                string[] Columns = { "nIDRFC", "nFolio" }; // Columnas que identifican registros únicos

                                Boolean result = _conexion.UpsertConBulkCopySimple(con, vdt.TableName, vdt, transaction, Columns);
                                if (!result)
                                {
                                    transaction.Rollback();
                                    cte = null;
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                                cte = null;
                            }
                        }

                        // Todo bien, commit
                        if (cte!=null)
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        cte = null;

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

            return cte;
        }

        public async Task<ContactoCliente> EliminarContactoCliente(ContactoCliente contacto)
        {
            ContactoCliente contactocliente = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_UP_CAT_EliminaContactoCliente_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nCliente", contacto.cliente);
                    cmd.Parameters.AddWithValue("@nContacto", contacto.contacto);

                    // Agrega el parámetro de retorno
                    var returnParameter = new SqlParameter
                    {
                        ParameterName = "@RETURN_VALUE",
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParameter);

                    await cmd.ExecuteNonQueryAsync();

                    int respuesta = (int)(returnParameter.Value ?? 0);

                    contactocliente = null;
                    if (respuesta == 1) {
                        contactocliente = contacto;
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo cancelar el contacto de cliente")
                {
                    Metodo = "EliminarContactoCliente",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return contactocliente;
        }

        private async Task<bool> EliminarContactosCliente(Int64 n_Cliente, SqlConnection externalConnection = null, SqlTransaction externalTransaction = null)
        {
            bool? respuesta = false;

            bool shouldCloseConnection = false;
            bool shouldCommitTransaction = false;

            SqlConnection con = externalConnection;
            SqlTransaction transaction = externalTransaction;

            try
            {
                if (con == null)
                {
                    con = _conexion.ObtenerSqlConexion();
                    await con.OpenAsync();
                    shouldCloseConnection = true;
                }

                if (transaction == null)
                {
                    transaction = con.BeginTransaction();
                    shouldCommitTransaction = true;
                }

                var cmd = new SqlCommand("DELETE CAT_ClientesContactos WHERE nCliente = @nCliente", con);
                cmd.CommandType = CommandType.Text; // Usamos CommandType.Text para una consulta SQL directa
                cmd.Transaction = transaction;
                // Agregamos el parámetro
                cmd.Parameters.AddWithValue("@nCliente", n_Cliente);

                // Ejecutamos el comando
                await cmd.ExecuteNonQueryAsync();

                respuesta = true;
                if (shouldCommitTransaction)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                if (shouldCommitTransaction)
                    transaction?.Rollback();

                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo eliminar los contactos del cliente")
                {
                    Metodo = "EliminarCorreosCliente",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return (bool)respuesta;
        }        

        public async Task<CatCorreoContactoRFC> EliminarCorreoCliente(CatCorreoContactoRFC correo)
        {
            CatCorreoContactoRFC correoCliente = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_UP_CAT_EliminaCorreoCliente_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nIDRFC", correo.IDRFC);
                    cmd.Parameters.AddWithValue("@cCorreoElectronico", correo.CorreoElectronico);

                    // Agrega el parámetro de retorno
                    var returnParameter = new SqlParameter
                    {
                        ParameterName = "@RETURN_VALUE",
                        Direction = ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(returnParameter);

                    await cmd.ExecuteNonQueryAsync();

                    int respuesta = (int)(returnParameter.Value ?? 0);

                    correoCliente = null;
                    if (respuesta == 1)
                    {
                        correoCliente = correo;
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo cancelar el contacto de cliente")
                {
                    Metodo = "EliminarCorreoCliente",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return correoCliente;
        }

        private async Task<bool> EliminarCorreosCliente(Int64 nIDRFC, SqlConnection externalConnection = null,SqlTransaction externalTransaction = null)
        {
            bool? respuesta = false;

            bool shouldCloseConnection = false;
            bool shouldCommitTransaction = false;

            SqlConnection con = externalConnection;
            SqlTransaction transaction = externalTransaction;

            try
            {
                if (con == null)
                {
                    con = _conexion.ObtenerSqlConexion();
                    await con.OpenAsync();
                    shouldCloseConnection = true;
                }

                if (transaction == null)
                {
                    transaction = con.BeginTransaction();
                    shouldCommitTransaction = true;
                }
               
                var cmd = new SqlCommand("DELETE CAT_CorreosContactoRFC WHERE nIDRFC = @nIDRFC", con);
                cmd.CommandType = CommandType.Text; // Usamos CommandType.Text para una consulta SQL directa
                cmd.Transaction = transaction;
                // Agregamos el parámetro
                cmd.Parameters.AddWithValue("@nIDRFC", nIDRFC);

                // Ejecutamos el comando
                await cmd.ExecuteNonQueryAsync();

                respuesta = true;
                if(shouldCommitTransaction)
                    transaction.Commit();
            }
            catch (Exception ex)
            {
                if (shouldCommitTransaction)
                    transaction?.Rollback();

                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo eliminar los contactos del cliente")
                {
                    Metodo = "EliminarCorreosCliente",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return (bool)respuesta;
        }

        private async Task<Int64> ObtenIdRFC_Cliente(Int64 n_Cliente)
        {
            Int64? nIDRFC = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT nIDRFC FROM CAT_Clientes (NOLOCK) where nCliente=@n_Cliente", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@n_Cliente", SqlDbType.BigInt).Value = n_Cliente;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            nIDRFC = ConvertUtils.ToInt64(reader["nIDRFC"]);

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
                throw new DataAccessException("Error(rp) No se pudo obtener el id rfc del cliente")
                {
                    Metodo = "ObtenIdRFC_Cliente",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return (long)nIDRFC;
        }
    }
}
