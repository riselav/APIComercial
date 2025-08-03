using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.API.Utils;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Dashboards.Indicadores;

namespace Voalaft.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]

    public class DashboardController : Controller
    {
        private readonly IDashboardIndicadoresServicio _dashboardIndicadoresServicio;

        private readonly ILogger<DashboardController> _logger;
        private readonly IConfiguration _config;

        public DashboardController(ILogger<DashboardController> logger, IConfiguration config,
                                    IDashboardIndicadoresServicio dashboardIndicadoresServicio)
        {
            _dashboardIndicadoresServicio = dashboardIndicadoresServicio;

            _logger = logger;
            _config = config;

        }

        [HttpPost("ObtenerDashboardIndicadores")]
        public async Task<ResultadoAPI> ObtenerDashboardIndicadores(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                DashboardIndicadores dashboardIndicadores = await _dashboardIndicadoresServicio.ObtenerDashboardIndicadores(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(dashboardIndicadores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de dashboard indicadores");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleVentasPorCategoria")]
        public async Task<ResultadoAPI> ObtenerDetalleVentasPorCategoria(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                SalesByCategoryDetail salesByCategoryDetail = await _dashboardIndicadoresServicio.ObtenerDetalleVentasPorCategoria(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(salesByCategoryDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de Ventas por Categoria");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleIngresosVsGastos")]
        public async Task<ResultadoAPI> ObtenerDetalleIngresosVsGastos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                IncomeExpensesDetailDto incomeExpensesDetail = await _dashboardIndicadoresServicio.ObtenerDetalleIngresosVsGastos(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(incomeExpensesDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de Ingresos Vs Gastos");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleFacturadoVsNoFacturado")]
        public async Task<ResultadoAPI> ObtenerDetalleFacturadoVsNoFacturado(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                InvoicedUninvoicedDetailDto invoicedUninvoicedDetail = await _dashboardIndicadoresServicio.ObtenerDetalleFacturadoVsNoFacturado(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(invoicedUninvoicedDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de facturado Vs No facturado");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleVentaPorFormaPago")]
        public async Task<ResultadoAPI> ObtenerDetalleVentaPorFormaPago(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                PaymentMethodDetail paymentMethodDetail = await _dashboardIndicadoresServicio.ObtenerDetalleVentaPorFormaPago(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(paymentMethodDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de venta por forma de pago");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleVentaPorTipoServicio")]
        public async Task<ResultadoAPI> ObtenerDetalleVentaPorTipoServicio(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                ServiceTypeDetail serviceTypeDetail = await _dashboardIndicadoresServicio.ObtenerDetalleVentaPorTipoServicio(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(serviceTypeDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de venta por tipo de servicio");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetalleVentaPorEstacionCocina")]
        public async Task<ResultadoAPI> ObtenerDetalleVentaPorEstacionCocina(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                KitchenStationDetail kitchenStationDetail = await _dashboardIndicadoresServicio.ObtenerDetalleVentaPorEstacionCocina(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(kitchenStationDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de venta por estación de cocina");
            }
            finally { }

            return resultado;
        }

        [HttpPost("ObtenerDetallePlatillosMasVendidos")]
        public async Task<ResultadoAPI> ObtenerDetallePlatillosMasVendidos(PeticionAPI peticion)
        {
            ResultadoAPI resultado = null;
            try
            {
                var r = CryptographyUtils.Desencriptar(peticion.contenido);
                var parametrosDashboardIndicadores = CryptographyUtils.DeserializarPeticion<ParametrosDashboardIndicadores>(r);
                TopSellingDetail platillosMasVendidos = await _dashboardIndicadoresServicio.ObtenerDetallePlatillosMasVendidos(parametrosDashboardIndicadores.Sucursal, parametrosDashboardIndicadores.FechaInicioNumero, parametrosDashboardIndicadores.FechaFinNumero);
                resultado = CryptographyUtils.CrearResultado(platillosMasVendidos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception("Error al consultar datos de detalle de platillos más vendidos");
            }
            finally { }

            return resultado;
        }
    }
}
