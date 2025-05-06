using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatalagosSATServicio
    {
        Task<List<CatUnidadesSat>> ListaCatUnidades();
        Task<CatUnidadesSat> ObtenerUnidadesPorClave(string cClave);
    }
}
