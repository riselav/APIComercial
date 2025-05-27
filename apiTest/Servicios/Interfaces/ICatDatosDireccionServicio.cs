using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatDatosDireccionServicio
    {
        Task<List<CatCodigosPostales>> ListaCodigosPostales();
        Task<List<CatColonias>> ObtenerColoniasPorCP(string cCodigoPostal);
    }
}
