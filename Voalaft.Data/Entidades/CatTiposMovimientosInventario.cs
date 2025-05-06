using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatTiposMovimientosInventario
    {
        public int nTipoMovimiento { get; set; }
        public string? cDescripcion { get; set; }
        public int nEfecto { get; set; }
        public bool bEsCancelacion { get; set; }
        public int nContramovimiento { get; set; }
        public int nTipoInvolucrado { get; set; }
        public  string? cEfecto { get; set; }
        public string? cTipoInvolucrado { get; set; }
        public string? cEsCancelacion { get; set; }
        public string? cContramovimiento { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
