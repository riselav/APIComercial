using Voalaft.API.Servicios.Implementacion;
using Voalaft.Data.Entidades;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatAlmacenesServicio
    {
        //Task<List<CatAlmacenes >> Lista(int nSucursal, int nPlaza, string Descripcion);
        //Task<List<CatAlmacenes>> Lista();
        Task<CatAlmacenes> ObtenerAlmacen(int nAlmacen);
        Task<CatAlmacenes> IME_CatAlmacenes(CatAlmacenes objAlmacen);
    }
}
