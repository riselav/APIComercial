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

        public long nFolio { get; set; }

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

        public List<ContactoCliente>? ContactoCliente { get; set;}

        public List<CatCorreoContactoRFC>? CorreosCliente { get; set; }

        // Método para generar el ID similar a SQL
        public long GenerateID()
        {
            // Convertir a cadenas y dar formato con ceros a la izquierda
            string? formattedSucursal = this.nSucursalRegistro?.ToString("D3"); // Formatea a 3 dígitos
            string formattedFolio = this.nFolio.ToString("D6"); // Formatea a 6 dígitos

            // Concatenar el resultado final y convertirlo a long
            string idString = "1" + formattedSucursal + formattedFolio;

            // Convertir la cadena a long
            return long.Parse(idString);
        }
    }
 }