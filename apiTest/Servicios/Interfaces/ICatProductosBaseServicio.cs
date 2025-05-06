using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatProductosBaseServicio
    {
        Task<List<CatProductosBase>> Lista();
        Task<CatProductosBase> ObtenerPorProductoBase(int nProductoBase);
        Task<CatProductosBase> IME_CatProductosBase(CatProductosBase catProductoBase);
    }
}
