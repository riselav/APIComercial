using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatTurnoServicio
    {
        Task<List<CatTurno>> Lista();
        Task<CatTurno> ObtenerPorId(long n_Turno);
    }
}