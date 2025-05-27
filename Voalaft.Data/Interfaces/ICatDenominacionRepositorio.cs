using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatDenominacionRepositorio
    {
        Task<List<CatDenominacion>> Lista();
        Task<CatDenominacion> ObtenerPorId(long n_Denominacion);
    }
}