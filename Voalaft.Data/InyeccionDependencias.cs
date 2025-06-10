using Microsoft.Extensions.DependencyInjection;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Implementaciones;
using Voalaft.Data.Interfaces;

namespace Voalaft.Data
{
    public static class InyeccionDependencias
    {
        public static void RegistrarRepositorios(this IServiceCollection services)
        {
            services.AddSingleton<Conexion>();
            services.AddTransient<IUsuariosRepositorio, UsuariosRepositorio>();
            services.AddTransient<ICatLineasRepositorio, CatLineasRepositorio>();
            services.AddTransient<ICatMarcasRepositorio, CatMarcasRepositorio>();
            services.AddTransient<ICatSublineasRepositorio, CatSublineasRepositorio>();
            services.AddTransient<ICatalagosSATRepositorio, CatalagosSATRepositorio>();
            services.AddTransient<ICatUnidadesRepositorio, CatUnidadesRepositorio>();
            services.AddTransient<ICatImpuestosRepositorio, CatImpuestosRepositorio>();
            services.AddTransient<ISatTipoFactorRepositorio, SatTipoFactorRepositorio>();
            services.AddTransient<ISatTasaOCuotaRepositorio, SatTasaOCuotaRepositorio>();
            services.AddTransient<ISatClaveProdServRepositorio, SatClaveProdServRepositorio>();
            services.AddTransient<ISatClaveUnidadRepositorio, SatClaveUnidadRepositorio>();
            services.AddTransient<ISatImpuestoRepositorio, SatImpuestoRepositorio>();
            services.AddTransient<ISatObjetoImpRepositorio, SatObjetoImpRepositorio>();
            services.AddTransient<ICatCatalogosRepositorio, CatCatalogosRepositorio>();
            services.AddTransient<ICatProductosBaseRepositorio, CatProductosBaseRepositorio>();
            services.AddTransient<ICatArticulosRepositorio, CatArticulosRepositorio>();
            services.AddTransient<ICatGrupoEmpresarialRepositorio, CatGrupoEmpresarialRepositorio>();
            services.AddTransient<ICatEmpresasRepositorio, CatEmpresasRepositorio>();
            services.AddTransient<ICatUnidadesRelacionalesRepositorio, CatUnidadesRelacionalesRepositorio>();
            services.AddTransient<ICatProveedoresRepositorio, CatProveedoresRepositorio>();
            services.AddTransient<ICatAlmacenesRepositorio, CatAlmacenesRepositorio >();
            services.AddTransient<ICatEmpleadosRepositorio, CatEmpleadosRepositorio >();
            services.AddTransient<ICatSucursalesRepositorio, CatSucursalesRepositorio >();
            services.AddTransient<ICatTiposMovimientosInventarioRepositorio , CatTiposMovimientosInventarioRepositorio >();

            services.AddTransient<IArticulosVentaRepositorio, ArticulosVentaRepositorio>();
            services.AddTransient<IRegMovimientoVentaRepositorio, RegMovimientoVentaRepositorio>();
            services.AddTransient<ISatRegimenFiscalRepositorio, SatRegimenFiscalRepositorio>();
            services.AddTransient<ISatCatUsoCFDIRepositorio, SatCatUsoCFDIRepositorio>();
            services.AddTransient<ICatRFCRepositorio, CatRFCRepositorio>();
            services.AddTransient<ICatCorreoContactoRFCRepositorio, CatCorreoContactoRFCRepositorio>();
            services.AddTransient<ICatCajaRepositorio, CatCajaRepositorio>();
            services.AddTransient<ICatClientesRepositorio, CatClientesRepositorio>();
            services.AddTransient<ICatProveedoresRepositorio, CatProveedoresRepositorio>();
            services.AddTransient<ICatDatosDireccionRepositorio, CatDatosDireccionRepositorio>();
            services.AddTransient<ICatTurnoRepositorio, CatTurnoRepositorio>();
            services.AddTransient<ICatDenominacionRepositorio, CatDenominacionRepositorio>();
            services.AddTransient<IRegAperturaCajaRepositorio, RegAperturaCajaRepositorio>();
            services.AddTransient<IRegMovimientoCajaRepositorio, RegMovimientoCajaRepositorio>();
            services.AddTransient<ICatFormaPagoRepositorio, CatFormaPagoRepositorio>();
            services.AddTransient<ICatTipoRegistroCajaRepositorio, CatTipoRegistroCajaRepositorio>();

            services.AddTransient<ICatConceptoCajaRepositorio, CatConceptoCajaRepositorio>();
        }
    }
}