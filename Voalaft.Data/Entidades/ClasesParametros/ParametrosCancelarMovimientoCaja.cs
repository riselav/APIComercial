using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.ClasesParametros
{
    public class ParametrosCancelarMovimientoCaja
    {
        public long aperturaCaja { get; set; }
        public int tipoRegistroCaja { get; set; }
        public int consecutivoRegistroCaja { get; set; }

        public long idRegistroCaja { get; set; }

        public string? usuarioCancela { get; set; }
        public string? maquinaCancela { get; set; }
        public string? usuarioAutoriza { get; set; }

    }
}
