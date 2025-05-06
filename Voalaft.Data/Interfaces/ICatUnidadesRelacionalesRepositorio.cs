
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatUnidadesRelacionalesRepositorio
    {
        Task<List<CatUnidadesRelacionales>> Lista();
        Task<CatUnidadesRelacionales> ObtenerPorUnidadRelacional(int nUnidadRelacional);
        Task<CatUnidadesRelacionales> IME_CatUnidadesRelacionales(CatUnidadesRelacionales catUnidadRelacional);
    }
}
