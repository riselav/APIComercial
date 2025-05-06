using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ISatObjetoImpServicio
    {
        Task<List<SatObjetoImp>> Lista();
        Task<SatObjetoImp> ObtenerPorObjetoImp(string c_ObjetoImp);
    }
}
