
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatDatosDireccionRepositorio
    {
        Task<List<CatCodigosPostales>> ListaCodigosPostales();
        Task<List<CatColonias>> ObtenerColoniasPorCP(string cCodigoPostal);
    }
}
