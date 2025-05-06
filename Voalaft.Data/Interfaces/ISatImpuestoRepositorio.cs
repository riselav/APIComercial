
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatImpuestoRepositorio
    {
        Task<List<SatImpuesto>> Lista();
        Task<SatImpuesto> ObtenerPorImpuesto(string c_Impuesto);
    }
}
