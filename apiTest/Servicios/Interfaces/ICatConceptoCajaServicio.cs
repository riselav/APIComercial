using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatConceptoCajaServicio
    {
        Task<List<CatConceptoCaja>> Lista();
        Task<CatConceptoCaja> ObtenerPorId(long n_ConceptoCaja);

        Task<List<CatConceptoCaja>> ObtenerPorEfecto(int n_Efecto);
    }
}
