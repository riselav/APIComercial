using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voalaft.Data.Entidades;

namespace Voalaft.Data.Interfaces
{
    public interface ICatConceptoCajaRepositorio
    {
        Task<List<CatConceptoCaja>> Lista();
        Task<CatConceptoCaja> ObtenerPorId(long n_ConceptoCaja);

        Task<List<CatConceptoCaja>> ObtenerPorEfecto(int n_Efecto);
    }
}