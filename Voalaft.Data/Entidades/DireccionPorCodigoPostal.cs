using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class DireccionPorCodigoPostal
    {
        public Entidad? Pais { get; set; }
        public Entidad? Estado { get; set; }
        public Entidad? Municipio { get; set; }
        public Entidad? Ciudad { get; set; }
        public List<Entidad>? Colonias { get; set; }
    }

    public class Entidad
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
    }

}
