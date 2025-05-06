using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatImpuestosSAT
    {
        public string c_impuesto { get; set; }
        public string Descripcion { get; set; }
        public bool EsRetencion { get; set; }
        public bool EsTraslado { get; set; }
    }
}
