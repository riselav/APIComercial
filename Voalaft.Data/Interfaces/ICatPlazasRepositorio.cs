using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatPlazasRepositorio
    {
        Task<List<CatPlaza>> Lista();

        Task<CatPlaza> ObtenerPorPlaza(int nPlaza);
    }
}