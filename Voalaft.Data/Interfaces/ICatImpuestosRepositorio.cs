
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatImpuestosRepositorio
    {
        Task<List<CatImpuestos>> Lista();
        Task<CatImpuestos> ObtenerPorImpuesto(int nImpuesto);
        Task<CatImpuestos> IME_CatImpuestos(CatImpuestos catImpuesto);
    }
}
