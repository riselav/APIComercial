using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatPlaza
    {
        public int plaza { get; set; }
        public string? descripcion { get; set; }
        public int region { get; set; }
        public bool activo { get; set; }
        public string? usuario { get; set; }
        public string? maquina { get; set; }
    }
}