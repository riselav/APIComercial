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
    public class CatAlmacenesRepositorio : ICatAlmacenesRepositorio
    {

        private readonly Conexion _conexion;
        private readonly ILogger<CatAlmacenesRepositorio> _logger;

        public CatAlmacenesRepositorio(ILogger<CatAlmacenesRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatAlmacenes> IME_CatAlmacenes(CatAlmacenes objAlmacen)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CAT_Almacenes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", objAlmacen.nAlmacen );
                    cmd.Parameters.AddWithValue("@cDescripcion", objAlmacen.cDescripcion);
                    cmd.Parameters.AddWithValue("@nPlaza", objAlmacen.nPlaza );
                    cmd.Parameters.AddWithValue("@nSucursal", objAlmacen.nSucursal );
                    cmd.Parameters.AddWithValue("@cCodigoPostal", objAlmacen.cCodigoPostal  );
                    cmd.Parameters.AddWithValue("@cColonia", objAlmacen.cColonia);
                    cmd.Parameters.AddWithValue("@cDomicilio", objAlmacen.cDomicilio );
                    cmd.Parameters.AddWithValue("@cTelefono1", objAlmacen.cTelefono1 );
                    cmd.Parameters.AddWithValue("@cTelefono2", objAlmacen.cTelefono2);
                    cmd.Parameters.AddWithValue("@bActivo", objAlmacen.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", objAlmacen.Usuario );
                    cmd.Parameters.AddWithValue("@cNombreMaquina", objAlmacen.Maquina );

                    await cmd.ExecuteNonQueryAsync();

                 }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (CatAlmacenesRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al obtener el almacén")
                {
                    Metodo = "CatAlmacenesRepositorio",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return objAlmacen ;
        }


        public async Task<CatAlmacenes> ObtenerAlmacen(int nAlmacen)
        {
            CatAlmacenes objAlmacen= null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Almacenes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", nAlmacen);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            objAlmacen =
                                new CatAlmacenes()
                                {
                                    nAlmacen = ConvertUtils.ToInt32(reader["nAlmacen"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cTelefono1 = ConvertUtils.ToString(reader["cTelefono1"]),
                                    cTelefono2 = ConvertUtils.ToString(reader["cTelefono2"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cNombrePlaza = ConvertUtils.ToString(reader["cNombrePlaza"]),
                                    cNombreSucursal = ConvertUtils.ToString(reader["cNombreSucursal"]),
                                    cNombreAsentamiento = ConvertUtils.ToString(reader["cNombreAsentamiento"]),
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

                _logger.LogError($"Error en {className}.{methodName} (CatAlmacenesRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al obtener el proveedor")
                {
                    Metodo = "ObtenerAlmacen",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return objAlmacen;
        }


        public async Task<List<CatAlmacenes >> Lista(ConsultaTabAlmacenes objFiltros)
        {
            List<CatAlmacenes> lstAlmacenes = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_Obten_Almacenes",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nSucursal", objFiltros.nSucursal);
                    cmd.Parameters.AddWithValue("@nPlaza", objFiltros.nPlaza);
                    //cmd.Parameters.AddWithValue("@CodigoPostal", CodigoPostal);
                    cmd.Parameters.AddWithValue("@Descripcion", objFiltros.cDescripcion);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstAlmacenes.Add(
                                new CatAlmacenes()
                                {
                                    nAlmacen = ConvertUtils.ToInt32(reader["nAlmacen"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nPlaza = ConvertUtils.ToInt32(reader["nPlaza"]),
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cTelefono1 = ConvertUtils.ToString(reader["cTelefono1"]),
                                    cTelefono2 = ConvertUtils.ToString(reader["cTelefono2"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cNombrePlaza = ConvertUtils.ToString(reader["cNombrePlaza"]),
                                    cNombreSucursal = ConvertUtils.ToString(reader["cNombreSucursal"]),
                                    cNombreAsentamiento = ConvertUtils.ToString(reader["cNombreAsentamiento"]),
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

                _logger.LogError($"Error en {className}.{methodName} (CatAlmacenesRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al obtener los almacenes")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return lstAlmacenes;
        }


    }
}
