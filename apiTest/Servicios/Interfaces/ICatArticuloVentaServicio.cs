using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IArticuloVentaServicio
    {

        //Task<List<CatArticuloVenta>> Lista();
    
        Task<List<CatArticuloVenta>> ObtenArticulosVenta(ParametrosObtenArticulosVenta parametrosObtenArticulosVenta);
        Task<List<CatArticuloVenta>> ObtenArticulosVentaV2(ParametrosObtenArticulosVenta parametrosObtenArticulosVenta);
    }
}