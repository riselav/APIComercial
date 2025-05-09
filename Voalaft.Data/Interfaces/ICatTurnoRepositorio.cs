using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatTurnoRepositorio
    {
        Task<List<CatTurno>> Lista();
        Task<CatTurno> ObtenerPorId(long n_Turno);
    }
}