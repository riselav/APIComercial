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
                    cmd.Parameters.AddWithValue("@nFolio", catSucursal.nSucursal );
                    cmd.Parameters.AddWithValue("@cDescripcion", catSucursal.cDescripcion);
                    cmd.Parameters.AddWithValue("@nEmpresa", catSucursal.nEmpresa );
                    cmd.Parameters.AddWithValue("@nPlaza", catSucursal.nPlaza);
                    cmd.Parameters.AddWithValue("@nRegion", catSucursal.nRegion );
                    cmd.Parameters.AddWithValue("@cEstado", catSucursal.cEstado );
                    cmd.Parameters.AddWithValue("@cLocalidad", catSucursal.cLocalidad );
                    cmd.Parameters.AddWithValue("@cMunicipio", catSucursal.cMunicipio );
                    cmd.Parameters.AddWithValue("@cCodigoPostal", catSucursal.cCodigoPostal );
                    cmd.Parameters.AddWithValue("@cColonia", catSucursal.cColonia);
                    cmd.Parameters.AddWithValue("@nZona", catSucursal.nZona );
                    cmd.Parameters.AddWithValue("@cDomicilio", catSucursal.cDomicilio );
                    cmd.Parameters.AddWithValue("@cTelefono1", catSucursal.cTelefono1 );
                    cmd.Parameters.AddWithValue("@cTelefono2", catSucursal.cTelefono2 );
                    cmd.Parameters.AddWithValue("@bActivo", catSucursal.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catSucursal.Usuario );
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catSucursal.Maquina );
                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catLinea.Linea = folioSig;
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
                                    nSucursal= ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cDescripcion= ConvertUtils.ToString(reader["cDescripcion"]),
                                    nEmpresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    nRegion = ConvertUtils.ToInt32(reader["nRegion"]),
                                    cEstado= ConvertUtils.ToString(reader["cEstado"]),
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
                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
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

            return Sucursales ;
        }


    }
}
