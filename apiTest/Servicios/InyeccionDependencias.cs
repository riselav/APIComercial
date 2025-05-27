using Voalaft.API.Servicios.Implementacion;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios
{
    public static class InyeccionDependencias
    {
        public static void RegistrarServicios(this IServiceCollection services)
        {
            services.AddTransient<IUsuariosServicio,UsuariosServicio>();
            services.AddTransient<ICatLineassServicio, CatLineassServicio>();
            services.AddTransient<ICatMarcasServicio, CatMarcasServicio>();
            services.AddTransient<ICatSublineasServicio, CatSublineasServicio>();
            services.AddTransient<ICatalagosSATServicio, CatalagosSATServicio>();
            services.AddTransient<ICatUnidadesServicio, CatUnidadesServicio>();
            services.AddTransient<ICatImpuestosServicio, CatImpuestosServicio>();
            services.AddTransient<ISatTasaOCuotaServicio, SatTasaOCuotaServicio>();
            services.AddTransient<ISatTipoFactorServicio, SatTipoFactorServicio>();
            services.AddTransient<ISatClaveProdServServicio, SatClaveProdServServicio>();
            services.AddTransient<ISatClaveUnidadServicio, SatClaveUnidadServicio>();
            services.AddTransient<ISatImpuestoServicio, SatImpuestoServicio>();
            services.AddTransient<ISatObjetoImpServicio, SatObjetoImpServicio>();
            services.AddTransient<ICatCatalogosServicio, CatCatalogosServicio>();
            services.AddTransient<ICatProductosBaseServicio, CatProductosBaseServicio>();
            services.AddTransient<ICatArticulosServicio, CatArticulosServicio>();
            services.AddTransient<ICatGrupoEmpresarialServicio, CatGrupoEmpresarialServicio>();
            services.AddTransient<ICatEmpresasServicio, CatEmpresasServicio>();
            services.AddTransient<ICatUnidadesRelacionalesServicio, CatUnidadesRelacionalesServicio>();
            services.AddTransient<ICatSucursalesServicio, CatSucursalesServicio  >();
            services.AddTransient<ICatProveedoresServicio, CatProveedoresServicio >();
            services.AddTransient<ICatAlmacenesServicio, CatAlmacenesServicio >();
            services.AddTransient<ICatEmpleadosServicio, CatEmpleadosServicio>();
            services.AddTransient<IArticuloVentaServicio, ArticulosVentaServicio>();
            services.AddTransient<IRegistroVentaServicio, RegMovimientoVentaServicio>();
            services.AddTransient<ISatRegimenFiscalServicio, SatRegimenFiscalServicio>();
            services.AddTransient<ISatCatUsoCFDIServicio, SatCatUsoCFDIServicio>();
            services.AddTransient<ICatRFCServicio, CatRFCServicio>();
            services.AddTransient<ICatCorreoContactoRFCServicio, CatCorreoContactoRFCServicio>();
            services.AddTransient<ICatCajaServicio, CatCajaServicio>();
            services.AddTransient<ICatClientesServicio, CatClientesServicio>();
            services.AddTransient<ICatSucursalesServicio, CatSucursalesServicio>();
            services.AddTransient<ICatProveedoresServicio, CatProveedoresServicio>();
            services.AddTransient<ICatDatosDireccionServicio, CatDatosDireccionServicio>();
        }
    }
}