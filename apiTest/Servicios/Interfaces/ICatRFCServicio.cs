using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatRFCServicio
    {
        Task<List<CatRFC>> Lista();
        Task<CatRFC> ObtenerPorRFC(string c_RFC);
    }
}