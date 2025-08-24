using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatSucursales
    {
        public int nSucursal { get; set; }
        public int nEmpresa { get; set; }
        public string? cDescripcion { get; set; }
        public int nPlaza { get; set; }
        public int nRegion { get; set; }
        public string? cEstado { get; set; }
        public string? cLocalidad { get; set; } 
        public string? cMunicipio { get; set; }
        public string? cCodigoPostal { get; set; }      
        public string? cColonia { get; set; }   
        public int nZona { get; set; }  
        public string? cDomicilio { get; set; } 
        public string? cTelefono1 { get; set; }
        public string? cTelefono2 { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }

        public string? NombreColonia { get; set; }

        public string? NombreEmpresa { get; set; }

        public string? NombrePlaza { get; set; }

        public int AlmacenInventario { get; set; }

        public List<string>? EncabezadoTicket { get; set; }

        public List<string>? PieTicket { get; set; }
    }
}