using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatEmpleados
    {
        public int Folio { get; set; }
        public string? cNombre { get; set; }
        public string? cApellidoPaterno { get; set; }
        public string? cApellidoMaterno { get; set; }
        public Int32 nEmpresa { get; set; }
        public DateTime  dFechaNacimiento { get; set; }
        public DateTime dFechaIngreso { get; set; }
        public DateTime dFechaBaja { get; set; }

        public string? cRFC { get; set; }
        public string? cCURP { get; set; }
        public string? cColonia { get; set; }
        public string? cCodigoPostal { get; set; }

        public string? cDomicilio { get; set; }

        public string? cReferencia { get; set; }

        public Int32 nSucursal { get; set; }

        public Int32 nPuesto { get; set; }

        public Int32 nDepartamento { get; set; }

        public string? cEstado { get; set; }

        public string? cMunicipio { get; set; }

        public string? cLocalidad { get; set; }

        public string? cAsentamiento { get; set; }

        public bool Activo { get; set; }

        public string? Usuario { get; set; }

        public string? Maquina { get; set; }

            
    }
}
