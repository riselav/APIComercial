﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class ParametrosConsultaCajas
    {
        public long Caja { get; set; }
        public long Sucursal { get; set; }

        public string? Descripcion { get; set; }
    }
}
