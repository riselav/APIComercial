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
    public class CatTiposMovimientosInventarioRepositorio: ICatTiposMovimientosInventarioRepositorio
    {

        private readonly Conexion _conexion;
        private readonly ILogger<CatTiposMovimientosInventarioRepositorio> _logger;

        public CatTiposMovimientosInventarioRepositorio(ILogger<CatTiposMovimientosInventarioRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<CatTiposMovimientosInventario> IME_CAT_TiposMovimientosInventario(CatTiposMovimientosInventario  catTipoMovimientoInv)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_IME_CAT_TiposMovimientosInventario",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", catTipoMovimientoInv.nTipoMovimiento );
                    cmd.Parameters.AddWithValue("@cDescripcion", catTipoMovimientoInv.cDescripcion);
                    cmd.Parameters.AddWithValue("@nEfecto", catTipoMovimientoInv.nEfecto);
                    cmd.Parameters.AddWithValue("@bEsCancelacion", catTipoMovimientoInv.bEsCancelacion );
                    cmd.Parameters.AddWithValue("@nContramovimiento", catTipoMovimientoInv.nContramovimiento );
                    cmd.Parameters.AddWithValue("@nTipoInvolucrado", catTipoMovimientoInv.nTipoInvolucrado );
                    cmd.Parameters.AddWithValue("@bActivo", catTipoMovimientoInv.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catTipoMovimientoInv.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catTipoMovimientoInv.Maquina);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo guardar el tipo de movimiento")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return catTipoMovimientoInv;
        }

        public async Task<CatTiposMovimientosInventario> ObtenerTipoMovimientoInventario(int nTipoMovimiento)
        {
            CatTiposMovimientosInventario  objTipoMovtoInv = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_TiposMovimientosInventario",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTipoMovimiento", nTipoMovimiento);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            objTipoMovtoInv =
                                new CatTiposMovimientosInventario()
                                {
                                    nTipoMovimiento  = ConvertUtils.ToInt32(reader["nTipoMovimiento"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nEfecto = ConvertUtils.ToInt32(reader["nEfecto"]),
                                    cEfecto = ConvertUtils.ToString(reader["cEfecto"]),
                                    bEsCancelacion = Convert.ToBoolean (reader["bEsCancelacion"]),
                                    cEsCancelacion = ConvertUtils.ToString(reader["cEsCancelacion"]),
                                    nContramovimiento = ConvertUtils.ToInt32(reader["nContramovimiento"]),
                                    cContramovimiento = ConvertUtils.ToString(reader["cContramovimiento"]),
                                    nTipoInvolucrado = ConvertUtils.ToInt32(reader["nTipoInvolucrado"]),
                                    cTipoInvolucrado = ConvertUtils.ToString(reader["cTipoInvolucrado"]),                               
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
                throw new DataAccessException("Error(rp) No se pudo obtener el tipo de movimiento de inventario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return objTipoMovtoInv;
        }

        public async Task<List<CatTiposMovimientosInventario>> Lista()
        {
            List<CatTiposMovimientosInventario> lstTiposMovtosInventario = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CAT_TiposMovimientosInventario",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nTipoMovimiento", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lstTiposMovtosInventario.Add(
                                new CatTiposMovimientosInventario()
                                {
                                    nTipoMovimiento = ConvertUtils.ToInt32(reader["nTipoMovimiento"]),
                                    cDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    nEfecto = ConvertUtils.ToInt32(reader["nEfecto"]),
                                    cEfecto = ConvertUtils.ToString(reader["cEfecto"]),
                                    bEsCancelacion = Convert.ToBoolean(reader["bEsCancelacion"]),
                                    cEsCancelacion = ConvertUtils.ToString(reader["cEsCancelacion"]),
                                    nContramovimiento = ConvertUtils.ToInt32(reader["nContramovimiento"]),
                                    cContramovimiento = ConvertUtils.ToString(reader["cContramovimiento"]),
                                    nTipoInvolucrado = ConvertUtils.ToInt32(reader["nTipoInvolucrado"]),
                                    cTipoInvolucrado = ConvertUtils.ToString(reader["cTipoInvolucrado"]),
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

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los tipos de movimientos de inventario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return lstTiposMovtosInventario ;
        }

    }
}
