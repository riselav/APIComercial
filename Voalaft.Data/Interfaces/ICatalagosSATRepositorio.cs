
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatalagosSATRepositorio
    {
        Task<List<CatUnidadesSat>> ListaCatUnidades();
        Task<CatUnidadesSat> ObtenerUnidadesPorClave(string cClave);
        List<CatImpuestosSAT> ObtenerListaCatImpuestosSAT();
        CatImpuestosSAT ObtenerCatImpuestoSATPorC_Impuesto(string c_impuesto);
        string ObtenerDescripcionCatImpuestoSATPorC_Impuesto(string c_impuesto);
    }
}
