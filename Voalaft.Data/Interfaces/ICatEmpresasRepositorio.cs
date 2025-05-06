
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatEmpresasRepositorio
    {
        Task<List<CatEmpresas>> Lista();
        Task<CatEmpresas> ObtenerPorEmpresa(int nEmpresa);
        Task<CatEmpresas> IME_CatEmpresas(CatEmpresas catEmpresa);
    }
}
