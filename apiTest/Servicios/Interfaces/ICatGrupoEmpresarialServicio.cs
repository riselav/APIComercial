using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatGrupoEmpresarialServicio
    {
        Task<List<CatGrupoEmpresarial>> Lista();
        Task<CatGrupoEmpresarial> ObtenerPorGrupoEmpresarial(int nGrupoEmpresarial);
        Task<CatGrupoEmpresarial> IME_CatGrupoEmpresarial(CatGrupoEmpresarial catGrupoEmpresarial);
    }
}
