using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{   
    public class CatClientes
    {
        public long nCliente { get; set; }

        public string? cNombreCompleto { get; set; }

        public string? cCalle { get; set; }

        public string? cNumExt { get; set; }

        public string? cNumInt { get; set; }

        public string? cColonia { get; set; }

        public string? cCodigoPostal { get; set; }                      

        public string? cTelefono { get; set; }

        public string? cSeniasParticulares { get; set; }

        public int? nSucursalRegistro { get; set; }
            
        public bool bManejaCredito { get; set; }

        public int? nTipoPersona { get; set; }

        public long? nIDRFC { get; set; }

        public bool Activo { get; set; }

        public string? Usuario { get; set; }

        public string? Maquina { get; set; }

        //***************************************
        public string? Estado { get; set; }
        public string? Municipio { get; set; }
        public string? NombreMunicipio { get; set; }

        public string? Localidad { get; set; }

        public string? NombreLocalidad { get; set; }

        public CatRFC? CatRFC { get; set; }

    }
    

}
