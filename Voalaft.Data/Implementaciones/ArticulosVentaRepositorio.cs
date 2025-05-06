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
    public class ArticulosVentaRepositorio : IArticulosVentaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<ArticulosVentaRepositorio> _logger;

        public ArticulosVentaRepositorio(ILogger<ArticulosVentaRepositorio> logger, Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;

        }

        public async Task<List<CatArticuloVenta>> ObtenArticulosVenta(ParametrosObtenArticulosVenta parametrosObtenArticulosVenta)
        {
            List<CatArticuloVenta> Articulos = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "VTA_ObtenArticulosVenta_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@prmSucursal", parametrosObtenArticulosVenta.Sucursal);
                    cmd.Parameters.AddWithValue("@prmListaPrecio", parametrosObtenArticulosVenta.ListaPrecio);
                    cmd.Parameters.AddWithValue("@Criterio", parametrosObtenArticulosVenta.Criterio);
        
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        int IdImpuestoIEPSIndex = reader.GetOrdinal("IdImpuestoIEPS");
                        int nombreIndex = reader.GetOrdinal("Descripcion");
                        int PorcentajeDescuentoUnitarioIndex = reader.GetOrdinal("PorcentajeDescuentoUnitario");
                        int ExistenciaIndex = reader.GetOrdinal("Existencia");

                        while (await reader.ReadAsync())
                        {
                            Articulos.Add(
                                new CatArticuloVenta()
                                {
                                    Articulo = ConvertUtils.ToInt32(reader["Articulo"]),
                                    Clave = ConvertUtils.ToString(reader["Clave"]),
                                    //Descripcion = ConvertUtils.ToString(reader["Descripcion"]),
                                    Descripcion = reader.IsDBNull(nombreIndex) ? "" : reader.GetString(nombreIndex), // solo por prueba, pues siempre tendra valor

                                    PrecioUnitarioNeto = ConvertUtils.ToDecimal(reader["PrecioUnitarioNeto"]),
                                    idSucursal = ConvertUtils.ToInt32(reader["idSucursal"]),
                                    idListaPrecio = ConvertUtils.ToInt32(reader["idListaPrecio"]),
                                    PrecioUnitarioSinImpuestos = ConvertUtils.ToDecimal(reader["PrecioUnitarioSinImpuestos"]),
                                    
                                    ImpuestoIVAUnitario = ConvertUtils.ToDecimal(reader["ImpuestoIVAUnitario"]),
                                    IdImpuestoIVA = ConvertUtils.ToInt32(reader["IdImpuestoIVA"]),
                                    PorcentajeImpuestoIVA = ConvertUtils.ToDecimal(reader["PorcentajeImpuestoIVA"]),

                                    ImpuestoIEPSUnitario = ConvertUtils.ToDecimal(reader["ImpuestoIEPSUnitario"]),
                                    //IdImpuestoIEPS = ConvertUtils.ToInt32(reader["IdImpuestoIEPS"]),
                                    //IdImpuestoIEPS = reader["IdImpuestoIEPS"] is DBNull ? 0 : ConvertUtils.ToInt32(reader["IdImpuestoIEPS"]),
                                    IdImpuestoIEPS = reader.IsDBNull(IdImpuestoIEPSIndex) ? 0 : reader.GetInt32(IdImpuestoIEPSIndex),
                                    PorcentajeImpuestoIEPS = ConvertUtils.ToDecimal(reader["PorcentajeImpuestoIEPS"]),

                                    esIEPSImporte = ConvertUtils.ToBoolean(reader["esIEPSImporte"]),
                                    
                                    CostoUnitario = ConvertUtils.ToDecimal(reader["CostoUnitario"]),
                                    PorcentajeDescuentoUnitario = reader.IsDBNull(PorcentajeDescuentoUnitarioIndex) ? 0 : reader.GetDecimal(PorcentajeDescuentoUnitarioIndex),
                                    Existencia = reader.IsDBNull(ExistenciaIndex) ? 0 : reader.GetDecimal(ExistenciaIndex),
                                    Activo = ConvertUtils.ToBoolean(reader["Activo"])
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
                throw new DataAccessException("Error(rp) No se pudo obtener los Articulos")
                {
                    Metodo = "ObtenArticulosVenta",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return Articulos;
        }
    }
}
