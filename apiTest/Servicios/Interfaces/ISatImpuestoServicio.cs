using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatImpuestoServicio
    {
        Task<List<SatImpuesto>> Lista();
        Task<SatImpuesto> ObtenerPorImpuesto(string c_Impuesto);
    }
}
