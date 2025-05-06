using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
        public class SatCatUsoCFDI
    {
        public string? UsoCFDI { get; set; }

        public string? Descripcion { get; set; }

        public bool? AplicaPersonaFisica { get; set; }

        public bool? AplicaPersonaMoral { get; set; }

        public DateTime? FechaInicioVigencia { get; set; }

        public string? RegimenFiscalReceptor { get; set; }
    }
}
