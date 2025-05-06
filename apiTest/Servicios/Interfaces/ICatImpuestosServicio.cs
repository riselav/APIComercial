using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatImpuestosServicio
    {
        Task<List<CatImpuestos>> Lista();
        Task<CatImpuestos> ObtenerPorImpuesto(int nImpuesto);
        Task<CatImpuestos> IME_CatImpuestos(CatImpuestos catImpuesto);
    }
}
