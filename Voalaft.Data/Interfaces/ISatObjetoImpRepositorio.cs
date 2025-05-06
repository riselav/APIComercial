
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ISatObjetoImpRepositorio
    {
        Task<List<SatObjetoImp>> Lista();
        Task<SatObjetoImp> ObtenerPorObjetoImp(string c_ObjetoImp);
    }
}
