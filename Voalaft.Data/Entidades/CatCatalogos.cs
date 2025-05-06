using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatCatalogos
    {
        public int Catalogo { get; set; }
        public string? Nombre { get; set; }
        public int Codigo { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }
}
