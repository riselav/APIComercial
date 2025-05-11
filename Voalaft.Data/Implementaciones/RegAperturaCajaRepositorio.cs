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

namespace Voalaft.Data.Implementaciones
{
    public class RegAperturaCajaRepositorio:IRegAperturaCajaRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<RegAperturaCajaRepositorio> _logger;
        private readonly IRegMovimientoCajaRepositorio _movimientoCajaRepositorio;

        public RegAperturaCajaRepositorio(ILogger<RegAperturaCajaRepositorio> logger, Conexion conexion, 
                                          IRegMovimientoCajaRepositorio movimientoCajaRepositorio)
        {
            _conexion = conexion;
            _logger = logger;
            _movimientoCajaRepositorio = movimientoCajaRepositorio;
        }

        public async Task<RegAperturaCaja> IME_REG_AperturaCaja(RegAperturaCaja regAperturaCaja)
        {
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand()
                    {
                        Connection = con,
                        CommandText = "CM_IME_CAJ_RegistroApertura",
                        CommandType = CommandType.StoredProcedure,
                    };
                    //cmd.Parameters.AddWithValue("@nFolio", regAperturaCaja.IDApertura);

                    SqlParameter outputParam = new SqlParameter("@nFolio", SqlDbType.BigInt);
                    outputParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outputParam);

                    cmd.Parameters.AddWithValue("@nSucursal", regAperturaCaja.IDSucursal);
                    cmd.Parameters.AddWithValue("@nCaja", regAperturaCaja.IDCaja);
                    cmd.Parameters.AddWithValue("@nTurno", regAperturaCaja.IDTurno);
                    cmd.Parameters.AddWithValue("@dFecha", regAperturaCaja.Fecha);

                    //cmd.Parameters.AddWithValue("@nDotacionInicial", regMovimientoVenta.nCotizacion == 0 ? null : regMovimientoVenta.nCotizacion);
                    cmd.Parameters.AddWithValue("@nDotacionInicial", regAperturaCaja.DotacionInicial);

                    cmd.Parameters.AddWithValue("@nEmpleado", regAperturaCaja.IDEmpleado);

                    cmd.Parameters.AddWithValue("@nUsuarioAutoriza", regAperturaCaja.IDUsuarioAutoriza == 0 ? null : regAperturaCaja.IDUsuarioAutoriza);

                    cmd.Parameters.AddWithValue("@nEstatus", regAperturaCaja.Estatus);
                    cmd.Parameters.AddWithValue("@bActivo", regAperturaCaja.Activo);

                    cmd.Parameters.AddWithValue("@cUsuario", regAperturaCaja.Usuario);
                    cmd.Parameters.AddWithValue("@cNombreMaquina", regAperturaCaja.Maquina);

                    //SqlParameter outputParam = new SqlParameter("@nVenta", SqlDbType.BigInt);
                    //outputParam.Direction = ParameterDirection.Output;
                    //cmd.Parameters.Add(outputParam);

                    await cmd.ExecuteNonQueryAsync();

                    //int folioSig = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                    //catMarca.Marca = folioSig;

                    regAperturaCaja.IDApertura = (long)cmd.Parameters["@nFolio"].Value;

                    if (regAperturaCaja.IDApertura > 0 && regAperturaCaja.regMovimientoCaja != null)
                    {
                        regAperturaCaja.regMovimientoCaja.IDApertura = regAperturaCaja.IDApertura;

                        await _movimientoCajaRepositorio.IME_REG_MovimientoCaja(regAperturaCaja.regMovimientoCaja);
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) al insertar apertura de caja")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return regAperturaCaja;
        }
    }
}