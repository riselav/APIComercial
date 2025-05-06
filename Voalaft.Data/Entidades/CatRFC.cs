using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
        public class CatRFC
    {
        public long nIDRFC { get; set; }

        public string? cRazonSocial { get; set; }

        public string? cRFC { get; set; }

        public string? cCP { get; set; }

        public string? cDomicilio { get; set; }

        public string? cUso_CFDI { get; set; }

        public string? cRegimenFiscal { get; set; }

        public bool? bActivo { get; set; }
    }
}
