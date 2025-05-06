
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Runtime.ConstrainedExecution;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

namespace Voalaft.Data.Implementaciones
{
    public class CatArticulosRepositorio : ICatArticulosRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<CatArticulosRepositorio> _logger;

        public CatArticulosRepositorio(ILogger<CatArticulosRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
            
        }

        public async Task<CatArticulos> IME_CatArticulos(CatArticulos catArticulo)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "RST_IME_CAT_Articulo",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nFolio", catArticulo.Articulo);
                    cmd.Parameters.AddWithValue("@cClave", catArticulo.Clave);
                    cmd.Parameters.AddWithValue("@cDescripcion", catArticulo.Descripcion);                    
                    cmd.Parameters.AddWithValue("@nUnidad", catArticulo.Unidad == 0 ? null : catArticulo.Unidad);
                    cmd.Parameters.AddWithValue("@cPresentacion", catArticulo.Presentacion);
                    cmd.Parameters.AddWithValue("@nLinea", catArticulo.Linea);
                    cmd.Parameters.AddWithValue("@nSublinea", catArticulo.Sublinea);                    
                    cmd.Parameters.AddWithValue("@cClaveSAT", catArticulo.ClaveSAT);
                    cmd.Parameters.AddWithValue("@bInsumoFinal", catArticulo.bInsumoFinal);
                    cmd.Parameters.Add("@nIdProductoBase", SqlDbType.Int).Value = catArticulo.IdProductoBase == 0 ? (object)DBNull.Value : catArticulo.IdProductoBase;                    
                    cmd.Parameters.Add("@nIdUnidadRelacional", SqlDbType.Int).Value = catArticulo.IdUnidadRelacional == 0 ? (object)DBNull.Value : catArticulo.IdUnidadRelacional;
                    cmd.Parameters.AddWithValue("@nEquivalencia", catArticulo.Equivalencia);
                    cmd.Parameters.Add("@nTipoArticulo", SqlDbType.Int).Value = catArticulo.TipoArticulo == 0 ? (object)DBNull.Value : catArticulo.TipoArticulo;
                    cmd.Parameters.AddWithValue("@bManejaInventario", catArticulo.bManejaInventario);
                    cmd.Parameters.AddWithValue("@bActivo", catArticulo.Activo);
                    cmd.Parameters.AddWithValue("@cUsuario", catArticulo.Usuario);
                    cmd.Parameters.AddWithValue("@nPrecioGeneral", catArticulo.PrecioGeneral);
                    cmd.Parameters.Add("@nIdImpuestoIVA", SqlDbType.Int).Value = catArticulo.IdImpuestoIVA == 0 ? (object)DBNull.Value : catArticulo.IdImpuestoIVA;
                    cmd.Parameters.Add("@nIdImpuestoIEPS", SqlDbType.Int).Value = catArticulo.IdImpuestoIEPS == 0 ? (object)DBNull.Value : catArticulo.IdImpuestoIEPS;                
                    cmd.Parameters.AddWithValue("@bDesglosaImpuestoIEPS", catArticulo.bDesglosaImpuestoIEPS);
                    cmd.Parameters.AddWithValue("@nImporteImpuestoIEPS", catArticulo.ImporteImpuestoIEPS);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", catArticulo.Maquina);
                    cmd.Parameters.AddWithValue("@nMarca", catArticulo.Marca);
                    cmd.Parameters.AddWithValue("@nIdListaPrecio", catArticulo.IdListaPrecio);
                    cmd.Parameters.AddWithValue("@nIdMoneda", catArticulo.IdMoneda);                    
                    cmd.Parameters.AddWithValue("@bLote", catArticulo.bLote);
                    cmd.Parameters.AddWithValue("@bSerie", catArticulo.bSerie);
                    cmd.Parameters.AddWithValue("@bPedimento", catArticulo.bPedimento);                    
                    cmd.Parameters.AddWithValue("@bProductoBase", catArticulo.bProductoBase);

                    SqlParameter returnValue = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;

                    await cmd.ExecuteNonQueryAsync();

                    int folio = (int)returnValue.Value; 

                    SqlCommand cmdDelPrecios = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "DELETE FROM CAT_Precios WHERE nIdArticulo="+ folio.ToString(),
                        CommandType = CommandType.Text,
                    };

                    await cmdDelPrecios.ExecuteNonQueryAsync();

                    if (folio > 0 && catArticulo.PreciosList != null && catArticulo.PreciosList.Count>0)
                    {
                        foreach(SucursalesCombo precio in catArticulo.PreciosList)
                        {
                            SqlCommand cmdInsPrecios = new SqlCommand()
                            {
                                Connection = con,
                                CommandText = "RST_IME_CAT_PrecioArticulo",
                                CommandType = CommandType.StoredProcedure,
                            };
                            cmdInsPrecios.Parameters.Add("@nIdSucursal", SqlDbType.Int).Value = precio.Sucursal == 0 || precio.Sucursal==null ? (object)DBNull.Value : precio.Sucursal;
                            cmdInsPrecios.Parameters.AddWithValue("@nIdArticulo", folio);
                            cmdInsPrecios.Parameters.AddWithValue("@nPrecio", precio.Precio);
                            cmdInsPrecios.Parameters.AddWithValue("@cUsuario", catArticulo.Usuario);
                            cmd.Parameters.AddWithValue("@cNombreMaquina", catArticulo.Maquina);

                            await cmdInsPrecios.ExecuteNonQueryAsync();
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

            return catArticulo;
        }

        public async Task<CatArticulos> ObtenerPorArticulo(int nArticulo)
        {
            CatArticulos articulo = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatArticulos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nIdArticulo", nArticulo);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            articulo=
                                new CatArticulos()
                                {
                                    Articulo = ConvertUtils.ToInt32(reader["nIDArticulo"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Clave = ConvertUtils.ToString(reader["cClave"]),
                                    Unidad = ConvertUtils.ToInt32(reader["nUnidad"]),
                                    UnidadDescripcion = ConvertUtils.ToString(reader["unidadDescripcion"]),
                                    Presentacion = ConvertUtils.ToString(reader["cPresentacion"]),
                                    Linea = ConvertUtils.ToInt32(reader["nLinea"]),
                                    LineaDescripcion = ConvertUtils.ToString(reader["lineaDescripcion"]),
                                    Sublinea = ConvertUtils.ToInt32(reader["nSublinea"]),
                                    sublineaDescripcion = ConvertUtils.ToString(reader["sublineaDescripcion"]),
                                    Marca = ConvertUtils.ToInt32(reader["nMarca"]),
                                    MarcaDescripcion = ConvertUtils.ToString(reader["marcaDescripcion"]),
                                    ClaveSAT = ConvertUtils.ToString(reader["cClaveSAT"]),
                                    bLote = ConvertUtils.ToBoolean(reader["bLote"]),
                                    bSerie = ConvertUtils.ToBoolean(reader["bSerie"]),
                                    bPedimento = ConvertUtils.ToBoolean(reader["bPedimento"]),
                                    bInsumoFinal = ConvertUtils.ToBoolean(reader["bInsumoFinal"]),
                                    bProductoBase = ConvertUtils.ToBoolean(reader["bProductoBase"]),
                                    IdProductoBase = ConvertUtils.ToInt32(reader["nIdProductoBase"]),
                                    ProductoBaseDescripcion = ConvertUtils.ToString(reader["productoBaseDescripcion"]),
                                    IdUnidadRelacional = ConvertUtils.ToInt32(reader["nIdUnidadRelacional"]),
                                    Equivalencia = ConvertUtils.ToDecimal(reader["nEquivalencia"]),
                                    bManejaInventario = ConvertUtils.ToBoolean(reader["bManejaInventario"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    IdImpuestoIVA = ConvertUtils.ToInt32(reader["IdImpuestoIVA"]),
                                    IdImpuestoIEPS = ConvertUtils.ToInt32(reader["IdImpuestoIEPS"]),
                                    ImporteImpuestoIEPS = ConvertUtils.ToInt32(reader["ImporteImpuestoIEPS"]),
                                    TipoArticulo = ConvertUtils.ToInt32(reader["nTipoArticulo"]),
                                };
                            break;
                        }
                    }
                    if(articulo!=null)
                    {
                        SqlCommand cmdPrecios = new SqlCommand()
                        {
                            Connection = con,
                            CommandText = "select p.nIdSucursal,s.cDescripcion,p.nPrecio from CAT_Precios p(nolock) left join CAT_Sucursales s(nolock) on p.nIdSucursal = s.nSucursal where nIDArticulo = " + nArticulo.ToString(),
                            CommandType = CommandType.Text,
                        };
                        List<SucursalesCombo> sucursales = [];
                        using (var readerPrecios = await cmdPrecios.ExecuteReaderAsync())
                        {
                            while (await readerPrecios.ReadAsync())
                            {
                                sucursales.Add(
                                    new SucursalesCombo()
                                    {
                                        Sucursal = ConvertUtils.ToInt32(readerPrecios["nIdSucursal"]) == 0 ? null : ConvertUtils.ToInt32(readerPrecios["nIdSucursal"]),
                                        SucursalDescripcion = ConvertUtils.ToString(readerPrecios["cDescripcion"]) == "" ? "PRECIO GENERAL" : ConvertUtils.ToString(readerPrecios["cDescripcion"]),
                                        Precio = ConvertUtils.ToString(readerPrecios["nPrecio"])
                                    }
                                    );
                            }
                        }
                        articulo.PreciosList = sucursales;                        
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

            return articulo;
        }

        public async Task<List<CatArticulos>> Lista()
        {
            List<CatArticulos> Articulos = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatArticulos",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@nArticulo", 0);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Articulos.Add(
                                new CatArticulos()
                                {
                                    Articulo = ConvertUtils.ToInt32(reader["nIDArticulo"]),
                                    Descripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Clave = ConvertUtils.ToString(reader["cClave"]),
                                    Unidad = ConvertUtils.ToInt32(reader["nUnidad"]),
                                    UnidadDescripcion = ConvertUtils.ToString(reader["unidadDescripcion"]),
                                    Presentacion = ConvertUtils.ToString(reader["cPresentacion"]),
                                    Linea = ConvertUtils.ToInt32(reader["nLinea"]),
                                    LineaDescripcion = ConvertUtils.ToString(reader["lineaDescripcion"]),
                                    Sublinea = ConvertUtils.ToInt32(reader["nSublinea"]),
                                    sublineaDescripcion = ConvertUtils.ToString(reader["sublineaDescripcion"]),
                                    Marca = ConvertUtils.ToInt32(reader["nMarca"]),
                                    MarcaDescripcion = ConvertUtils.ToString(reader["marcaDescripcion"]),
                                    ClaveSAT = ConvertUtils.ToString(reader["cClaveSAT"]),
                                    bLote = ConvertUtils.ToBoolean(reader["bLote"]),
                                    bSerie = ConvertUtils.ToBoolean(reader["bSerie"]),
                                    bPedimento = ConvertUtils.ToBoolean(reader["bPedimento"]),
                                    bInsumoFinal = ConvertUtils.ToBoolean(reader["bInsumoFinal"]),
                                    bProductoBase = ConvertUtils.ToBoolean(reader["bProductoBase"]),
                                    IdProductoBase = ConvertUtils.ToInt32(reader["nIdProductoBase"]),
                                    ProductoBaseDescripcion = ConvertUtils.ToString(reader["productoBaseDescripcion"]),
                                    IdUnidadRelacional = ConvertUtils.ToInt32(reader["nIdUnidadRelacional"]),
                                    Equivalencia = ConvertUtils.ToDecimal(reader["nEquivalencia"]),
                                    bManejaInventario = ConvertUtils.ToBoolean(reader["bManejaInventario"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    IdImpuestoIVA = ConvertUtils.ToInt32(reader["IdImpuestoIVA"]),
                                    IdImpuestoIEPS = ConvertUtils.ToInt32(reader["IdImpuestoIEPS"]),
                                    ImporteImpuestoIEPS = ConvertUtils.ToInt32(reader["ImporteImpuestoIEPS"]),
                                    TipoArticulo = ConvertUtils.ToInt32(reader["nTipoArticulo"]),
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
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return Articulos;
        }

        public async Task<List<ConsultaArticulo>> ConsultaTableroArticulos(ParametrosConsultaArticulo parametrosConsultaArticulo )
        {
            List<ConsultaArticulo> Articulos = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CAT_CON_CatArticulos_SP",
                        CommandType = CommandType.StoredProcedure,
                    };
                    cmd.Parameters.AddWithValue("@prmMarca", parametrosConsultaArticulo.Marca);
                    cmd.Parameters.AddWithValue("@prmLinea", parametrosConsultaArticulo.Linea);
                    cmd.Parameters.AddWithValue("@prmSublinea", parametrosConsultaArticulo.Sublinea);
                    cmd.Parameters.AddWithValue("@prmTipoUnidad", parametrosConsultaArticulo.TipoUnidad);
                    cmd.Parameters.AddWithValue("@prmUnidad", parametrosConsultaArticulo.Unidad);
                    cmd.Parameters.AddWithValue("@prmArticulo", parametrosConsultaArticulo.Articulo);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Articulos.Add(
                                new ConsultaArticulo()
                                {
                                    IdArticulo = ConvertUtils.ToInt32(reader["nIdArticulo"]),
                                    Clave = ConvertUtils.ToString(reader["cClave"]),
                                    NombreArticulo = ConvertUtils.ToString(reader["cNombreArticulo"]),
                                    NombreMarca = ConvertUtils.ToString(reader["cNombreMarca"]),
                                    NombreLinea = ConvertUtils.ToString(reader["cNombreLinea"]),
                                    NombreSublinea= ConvertUtils.ToString(reader["cNombreSubLinea"]),
                                    UnidadInventario = ConvertUtils.ToString(reader["cUnidadInventario"]),
                                    TipoArticulo = ConvertUtils.ToString(reader["cTipoArticulo"]),
                                    TipoUnidad = ConvertUtils.ToString(reader["cTipoUnidad"]),
                                    ManejaInventario = ConvertUtils.ToString(reader["cManejaInventario"]),
                                    InsumoFinal = ConvertUtils.ToString(reader["cInsumoFinal"]),
                                    ProductoBase = ConvertUtils.ToString(reader["cProductoBase"]),

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
                    Metodo = "ConsultaTableroArticulos",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return Articulos;
        }

        public async Task<List<SucursalesCombo>> ListaSucursales()
        {
            List<SucursalesCombo> sucursales = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "SELECT nSucursal,cDescripcion FROM CAT_Sucursales where bActivo=1",
                        CommandType = CommandType.Text,
                    };
                    sucursales.Add(
                                new SucursalesCombo()
                                {
                                    Sucursal = null,
                                    SucursalDescripcion = "PRECIO GENERAL",
                                    Precio=""
                                }
                                );
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            sucursales.Add(
                                new SucursalesCombo()
                                {
                                    Sucursal= ConvertUtils.ToInt32(reader["nSucursal"]),
                                    SucursalDescripcion = ConvertUtils.ToString(reader["cDescripcion"]),
                                    Precio = ""
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
                throw new DataAccessException("Error(rp) No se pudo obtener las sucursales")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return sucursales;
        }
    }
}
