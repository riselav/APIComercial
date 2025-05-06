
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatGrupoEmpresarialRepositorio
    {
        Task<List<CatGrupoEmpresarial>> Lista();
        Task<CatGrupoEmpresarial> ObtenerPorGrupoEmpresarial(int nGrupoEmpresarial);
        Task<CatGrupoEmpresarial> IME_CatGrupoEmpresarial(CatGrupoEmpresarial catGrupoEmpresarial);
    }
}
