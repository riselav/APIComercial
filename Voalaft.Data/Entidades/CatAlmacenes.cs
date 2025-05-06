using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatAlmacenes
    {

        public Int32 nAlmacen { get; set; }        
        public string? cDescripcion { get; set; }
        public Int32 nPlaza { get; set; }
        public Int32 nSucursal { get; set; }
        public string? cCodigoPostal {  get; set; }
        public string? cColonia { get; set; }
        public string? cDomicilio { get; set; }
        public string? @cTelefono1 { get; set; }
        public string? @cTelefono2 { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
        public string? cNombrePlaza { get; set; }
        public string? cNombreSucursal { get; set; }
        public string? cNombreAsentamiento { get; set; }


    }
}
