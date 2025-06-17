using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegCorteCajaServicio
    {
        Task<RegCorteCaja> IME_REG_CorteCaja(RegCorteCaja regCorteCaja);
    }
}