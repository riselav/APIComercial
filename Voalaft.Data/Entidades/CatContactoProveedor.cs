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

        public int nContactoProveedor { get; set; }

        public string? cContacto { get; set; }

        public string? cPuesto { get; set; }

        public string? cTelefono { get; set; }

        public string? cTelefonoAux { get; set; }

        public string? cCorreo { get; set; }

        public bool bActivo { get; set; }

        public string? Usuario { get; set; }

        public string? Maquina { get; set; }

        public DateTime? Fecha { get; set; }

            }
}
