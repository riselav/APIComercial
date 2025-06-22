using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;

namespace Voalaft.Data.Interfaces
{
    public interface ICatClientesRepositorio
    {
        Task<List<CatClientes>> Lista();
        Task<CatClientes> ObtenerPorId(long n_Cliente);

        Task<List<Cliente>> ConsultaClientes(ParametrosConsultaClientes paramClientes);
    }
}