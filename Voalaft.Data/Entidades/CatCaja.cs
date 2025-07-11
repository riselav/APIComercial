using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatCaja
    {
        public int Caja { get; set; }
        public string? Descripcion { get; set; }

        public int Sucursal { get; set; }

        public string? NombreSucursal { get; set; }

        public int Impresora { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}