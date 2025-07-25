using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Consultas
{
    public class ImpresionData
    {
        public int nRenglon { get; set; }
        public int nRenglonConcepto { get; set; }
        public int nRenglonMod { get; set; }
        public string cLinea { get; set; }
        public bool bLetraGrande { get; set; }
        public bool bNegrita { get; set; }
        public bool bCodBarra { get; set; }
    }
}
