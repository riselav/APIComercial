using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class RegDenominacionCorteCaja : CatDenominacion
    {
        public decimal Cantidad { get; set; }
        public decimal Importe { get; set; }
        public int Renglon { get; set; } 
    }
}