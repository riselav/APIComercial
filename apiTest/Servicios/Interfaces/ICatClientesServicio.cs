using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.ClasesParametros;
using Voalaft.Data.Entidades.Tableros;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface ICatClientesServicio
    {
        Task<List<CatClientes>> Lista();
        Task<CatClientes> ObtenerPorId(long n_Cliente);

        Task<List<Cliente>> ConsultaClientes(ParametrosConsultaClientes paramClientes);

        Task<CatClientes> IME_Cliente(CatClientes cliente);

        Task<ContactoCliente> EliminarContactoCliente(ContactoCliente contacto);
    }
}