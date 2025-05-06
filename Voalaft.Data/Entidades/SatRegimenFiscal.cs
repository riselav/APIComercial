using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class SatRegimenFiscal
    {
        public long? nIdRegimenFiscal { get; set; }
        public string? RegimenFiscalDescripcion { get; set; }

        public bool bFisica { get; set; }

        public bool bMoral { get; set; }
    }
}