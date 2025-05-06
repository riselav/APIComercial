using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class ParametrosObtenArticulosVenta
    {
        public int Sucursal { get; set; }
        public int ListaPrecio { get; set; }
        public string? Criterio { get; set; }
    }
}