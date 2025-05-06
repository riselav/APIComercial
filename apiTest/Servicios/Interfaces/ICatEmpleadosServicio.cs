using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatEmpleadosServicio
    {
        Task<List<CatEmpleados >> Lista();
        Task<CatEmpleados> ObtenerEmpleado (int nEmpleado);
        Task<CatEmpleados> IME_CAT_Empleados(CatEmpleados catEmpleado);
    }
}
