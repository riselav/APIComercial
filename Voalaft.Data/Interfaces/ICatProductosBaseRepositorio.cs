
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatProductosBaseRepositorio
    {
        Task<List<CatProductosBase>> Lista();
        Task<CatProductosBase> ObtenerPorProductoBase(int nProductoBase);
        Task<CatProductosBase> IME_CatProductosBase(CatProductosBase catProductoBase);
    }
}
