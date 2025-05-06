using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatCatUsoCFDIServicio
    {
        Task<List<SatCatUsoCFDI>> Lista();
        Task<SatCatUsoCFDI> ObtenerPorClave(string c_UsoCFDI);
    }
}