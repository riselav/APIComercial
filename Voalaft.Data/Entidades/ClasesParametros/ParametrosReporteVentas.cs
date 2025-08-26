using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.ClasesParametros
{
    public class ParametrosReporteVentas
    {
        public int? nCajero { get; set; }
        public bool? bEstatus { get; set; }
        public int? nTipoVenta { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public string? cBusquedaGeneral { get; set; }
    }
}
