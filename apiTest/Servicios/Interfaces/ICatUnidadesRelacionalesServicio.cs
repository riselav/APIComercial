using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatUnidadesRelacionalesServicio
    {
        Task<List<CatUnidadesRelacionales>> Lista();
        Task<CatUnidadesRelacionales> ObtenerPorUnidadRelacional(int nUnidadRelacional);
        Task<CatUnidadesRelacionales> IME_CatUnidadesRelacionales(CatUnidadesRelacionales catUnidadRelacional);
    }
}
