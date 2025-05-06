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
    public class CatEmpleadosRepositorio: ICatEmpleadosRepositorio

    {

        private readonly Conexion _conexion;
        private readonly ILogger<CatEmpleadosRepositorio> _logger;

        public CatEmpleadosRepositorio(ILogger<CatEmpleadosRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }


        public async Task<CatEmpleados > IME_CAT_Empleados(CatEmpleados  catEmpleado)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "IME_CAT_Empleados",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", catEmpleado.Folio );
                    cmd.Parameters.AddWithValue("@cNombre", catEmpleado.cNombre);
                    cmd.Parameters.AddWithValue("@cApellidoPaterno", catEmpleado.cApellidoPaterno);
                    cmd.Parameters.AddWithValue("@cApellidoMaterno", catEmpleado.cApellidoMaterno);
                    cmd.Parameters.AddWithValue("@nEmpresa", catEmpleado.nEmpresa );
                    cmd.Parameters.AddWithValue("@dFechaNacimiento", catEmpleado.dFechaNacimiento );
                    cmd.Parameters.AddWithValue("@dFechaIngreso", catEmpleado.dFechaIngreso );
                    cmd.Parameters.AddWithValue("@dFechaBaja", catEmpleado.dFechaBaja );
                    cmd.Parameters.AddWithValue("@cRFC", catEmpleado.cRFC);
                    cmd.Parameters.AddWithValue("@cCURP", catEmpleado.cCURP );
                    cmd.Parameters.AddWithValue("@cCveColonia", catEmpleado.cColonia );
                    cmd.Parameters.AddWithValue("@cCodigoPostal", catEmpleado.cCodigoPostal );
                    cmd.Parameters.AddWithValue("@cDomicilio", catEmpleado.cDomicilio );
                    cmd.Parameters.AddWithValue("@cReferencia", catEmpleado.cReferencia );
                    cmd.Parameters.AddWithValue("@nSucursal", catEmpleado.nSucursal );
                    cmd.Parameters.AddWithValue("@nPuesto", catEmpleado.nPuesto );
                    cmd.Parameters.AddWithValue("@nDepartamento", catEmpleado.nDepartamento );
                    cmd.Parameters.AddWithValue("@bActivo", catEmpleado.Activo );
                    cmd.Parameters.AddWithValue("@cUsuario", catEmpleado.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catEmpleado.Maquina );
                   
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al guardar el empleado")
                {
                    Metodo = "IME_CAT_Empleados",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catEmpleado ;
        }

        public async Task<CatEmpleados> ObtenerEmpleado(int nEmpleado)
        {
            CatEmpleados objEmpleado= null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Empleados",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", nEmpleado );
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            objEmpleado =
                                new CatEmpleados ()
                                {
                                    Folio  = ConvertUtils.ToInt32(reader["nEmpleado"]),
                                    cNombre = ConvertUtils.ToString(reader["cNombre"]),
                                    cApellidoPaterno  = ConvertUtils.ToString (reader["cApellidoPaterno"]),
                                    cApellidoMaterno  = ConvertUtils.ToString(reader["cApellidoMaterno"]),
                                    nEmpresa = ConvertUtils.ToInt32 (reader["nEmpresa"]),
                                    dFechaNacimiento = ConvertUtils.ToDateTime (reader["dFechaNacimiento"]),
                                    dFechaIngreso = ConvertUtils.ToDateTime(reader["dFechaIngreso"]),
                                    dFechaBaja = ConvertUtils.ToDateTime(reader["dFechaBaja"]),
                                    cRFC = ConvertUtils.ToString(reader["cRFC"]),
                                    cCURP = ConvertUtils.ToString(reader["cCURP"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cReferencia = ConvertUtils.ToString(reader["cReferencia"]),
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    nPuesto = ConvertUtils.ToInt32(reader["nPuesto"]),
                                    nDepartamento = ConvertUtils.ToInt32(reader["nDepartamento"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    cEstado = ConvertUtils.ToString(reader["cNombreEstado"]),
                                    cMunicipio = ConvertUtils.ToString(reader["cNombreMunicipio"]),
                                    cLocalidad = ConvertUtils.ToString(reader["cNombreLocalidad"]),
                                    cAsentamiento = ConvertUtils.ToString(reader["cNombreAsentamiento"])                                  
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al obtener el empleado")
                {
                    Metodo = "ObtenerEmpleado",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return objEmpleado;
        }


        public async Task<List<CatEmpleados>> Lista()
        {
            List<CatEmpleados> lstEmpleados = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_Empleados",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstEmpleados.Add(
                                new CatEmpleados ()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nEmpleado"]),
                                    cNombre = ConvertUtils.ToString(reader["cNombre"]),
                                    cApellidoPaterno = ConvertUtils.ToString(reader["cApellidoPaterno"]),
                                    cApellidoMaterno = ConvertUtils.ToString(reader["cApellidoMaterno"]),
                                    nEmpresa = ConvertUtils.ToInt32(reader["nEmpresa"]),
                                    dFechaNacimiento = ConvertUtils.ToDateTime(reader["dFechaNacimiento"]),
                                    dFechaIngreso = ConvertUtils.ToDateTime(reader["dFechaIngreso"]),
                                    dFechaBaja = ConvertUtils.ToDateTime(reader["dFechaBaja"]),
                                    cRFC = ConvertUtils.ToString(reader["cRFC"]),
                                    cCURP = ConvertUtils.ToString(reader["cCURP"]),
                                    cColonia = ConvertUtils.ToString(reader["cColonia"]),
                                    cCodigoPostal = ConvertUtils.ToString(reader["cCodigoPostal"]),
                                    cDomicilio = ConvertUtils.ToString(reader["cDomicilio"]),
                                    cReferencia = ConvertUtils.ToString(reader["cReferencia"]),
                                    nSucursal = ConvertUtils.ToInt32(reader["nSucursal"]),
                                    nPuesto = ConvertUtils.ToInt32(reader["nPuesto"]),
                                    nDepartamento = ConvertUtils.ToInt32(reader["nDepartamento"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    cEstado = ConvertUtils.ToString(reader["cNombreEstado"]),
                                    cMunicipio = ConvertUtils.ToString(reader["cNombreMunicipio"]),
                                    cLocalidad = ConvertUtils.ToString(reader["cNombreLocalidad"]),
                                    cAsentamiento = ConvertUtils.ToString(reader["cNombreAsentamiento"])
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosRepositorio {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al obtener los empleados ")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return lstEmpleados ;
        }

     
    }
}
