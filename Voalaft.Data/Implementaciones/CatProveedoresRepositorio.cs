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
    internal class CatProveedoresRepositorio: ICatProveedoresRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatProveedoresRepositorio> _logger;

        public CatProveedoresRepositorio(ILogger<CatProveedoresRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatProveedores> IME_CatProveedores (CatProveedores  catProveedor )
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_Proveedores",
                        CommandType = CommandType.StoredProcedure,
                    };

                    cmd.Parameters.AddWithValue("@nProveedor", catProveedor.Folio);
                    cmd.Parameters.AddWithValue("@cDescripcionComercial", catProveedor.DescripcionComercial);
                    cmd.Parameters.AddWithValue("@cDescripcionFiscal", catProveedor.DescripcionFiscal);
                    cmd.Parameters.AddWithValue("@cColonia", catProveedor.cColonia);
                    cmd.Parameters.AddWithValue("@cCodigoPostal", catProveedor.cCodigoPostal);
                    cmd.Parameters.AddWithValue("@cRFC", catProveedor.cRFC );
                    cmd.Parameters.AddWithValue("@cCURP", catProveedor.cCURP );
                    cmd.Parameters.AddWithValue("@nTipoPersona", catProveedor.nTipoPersona );
                    cmd.Parameters.AddWithValue("@bNacional", catProveedor.bNacional );
                    cmd.Parameters.AddWithValue("@nDiasCredito", catProveedor.nDiasCredito );
                    cmd.Parameters.AddWithValue("@cTelefono", catProveedor.cTelefono );
                    cmd.Parameters.AddWithValue("@cCorreo", catProveedor.cCorreo );
                    cmd.Parameters.AddWithValue("@bActivo", catProveedor.bActivo );
                    cmd.Parameters.AddWithValue("@cUsuario", catProveedor.cUsuario );
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catProveedor.cMaquina );

                    int proveedorId=  await cmd.ExecuteNonQueryAsync();
                    int Renglon = 1;
                    if (catProveedor.lstContactosProveedores.Count > 0)
                    {
                        DataTable vdt = _conexion.ObtenerEsquemaTabla("CAT_ProveedoresContactos");

                        Renglon = 1;
                        foreach (CatContactoProveedor  contacto in catProveedor.lstContactosProveedores)
                        {
                            contacto.nProveedor = proveedorId;
                            vdt.Rows.Add(contacto.nProveedor, 
                                Renglon, 
                                contacto.cNombre, 
                                contacto.cPuesto, 
                                contacto.cTelefono,
                                contacto.cCelular ,
                                contacto.cCorreoElectronico,
                                contacto.nTipoContacto,
                                contacto.Activo,
                                contacto.Usuario,
                                contacto.Maquina,
                                contacto.Fecha
                                );

                            Renglon += 1;
                        }

                        Boolean result = _conexion.InsertarConBulkCopy(con, vdt.TableName, vdt);



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

            return catProveedor ;
        }

    public async Task<CatProveedores> ObtenerProveedor(int nProveedor)
    {
        CatProveedores Proveedor = null;
        try
        {
            using (var con = _conexion.ObtenerSqlConexion())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandText = "CAT_CON_CAT_Proveedores",
                    CommandType = CommandType.StoredProcedure,
                };
                cmd.Parameters.AddWithValue("@nProveedor", nProveedor );
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Proveedor  =
                            new CatProveedores ()
                            {
                                Folio = ConvertUtils.ToInt32(reader["nProveedor"]),
                                DescripcionComercial = ConvertUtils.ToString(reader["cDescripcionComercial"]),
                                DescripcionFiscal = ConvertUtils.ToString(reader["cDescripcionFiscal"]),
                                cColonia = ConvertUtils.ToString (reader["cColonia"]),
                                cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                cRFC =   ConvertUtils.ToString(reader["cRFC"]),
                                cCURP = ConvertUtils.ToString(reader["cCURP"]),
                                nTipoPersona = ConvertUtils.ToInt32 (reader["nTipoPersona"]),
                                bNacional = ConvertUtils.ToBoolean (reader["bNacional"]),
                                nDiasCredito = ConvertUtils.ToInt32 (reader["nDiasCredito"]),
                                cTelefono = ConvertUtils.ToString(reader["cTelefono"]),
                                cCorreo = ConvertUtils.ToString(reader["cCorreo"]),
                                bActivo = ConvertUtils.ToBoolean (reader["bActivo"])
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

        return Proveedor ;
    }


        public async Task<List<CatProveedores >> Lista()
        {
            List<CatProveedores> LstProveedores = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Proveedores",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nProveedor", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            LstProveedores.Add(
                                new CatProveedores()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nProveedor"]),
                                    DescripcionComercial = ConvertUtils.ToString(reader["cDescripcionComercial"]),
                                    DescripcionFiscal = ConvertUtils.ToString(reader["cDescripcionFiscal"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cRFC = ConvertUtils.ToString(reader["cRFC"]),
                                    cCURP = ConvertUtils.ToString(reader["cCURP"]),
                                    nTipoPersona = ConvertUtils.ToInt32(reader["nTipoPersona"]),
                                    bNacional = ConvertUtils.ToBoolean(reader["bNacional"]),
                                    nDiasCredito = ConvertUtils.ToInt32(reader["nDiasCredito"]),
                                    cTelefono = ConvertUtils.ToString(reader["cTelefono"]),
                                    cCorreo = ConvertUtils.ToString(reader["cCorreo"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los proveedores")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return LstProveedores ;
        }

          }
}