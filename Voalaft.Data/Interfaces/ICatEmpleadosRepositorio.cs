using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Implementaciones;

namespace Voalaft.Data.Interfaces
{
    public interface ICatEmpleadosRepositorio  

    {
        Task<List<CatEmpleados >> Lista();
        Task<CatEmpleados> ObtenerEmpleado(int nEmpleado);
        Task<CatEmpleados> IME_CAT_Empleados(CatEmpleados catEmpleado);
    }
}
