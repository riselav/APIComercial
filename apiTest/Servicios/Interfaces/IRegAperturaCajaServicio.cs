using Voalaft.Data.Entidades;

namespace Voalaft.API.Servicios.Interfaces
{
    public interface IRegAperturaCajaServicio
    {
        Task<RegAperturaCaja> IME_REG_AperturaCaja(RegAperturaCaja regAperturaCaja);

        Task<RegAperturaCaja> ObtenAperturaAbierta(RegAperturaCaja regAperturaCaja);
    }
}