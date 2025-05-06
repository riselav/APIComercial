using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatEmpresasServicio
    {
        Task<List<CatEmpresas>> Lista();
        Task<CatEmpresas> ObtenerPorEmpresa(int nEmpresa);
        Task<CatEmpresas> IME_CatEmpresas(CatEmpresas catEmpresa);
    }
}
