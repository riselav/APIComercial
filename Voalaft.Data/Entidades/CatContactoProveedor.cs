using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatContactoProveedor
    {
        public int nProveedor { get; set; }

        public int nContacto { get; set; }

        public string? cNombre { get; set; }

        public string? cPuesto { get; set; }

        public string? cTelefono { get; set; }

        public string? cCelular { get; set; }

        public string? cCorreoElectronico { get; set; }

        public int nTipoContacto { get; set; }

        public bool Activo { get; set; }

        public string? Usuario { get; set; }

        public string? Maquina { get; set; }

        public DateTime Fecha { get; set; }

            }
}
