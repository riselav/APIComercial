using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Tableros
{
    public class TableroProveedores
    {
        public int Proveedor { get; set; }
        public string? DescripcionComercial { get; set; }
        public string? DescripcionColonia { get; set; }
        public string? NombreMunicipio { get; set; }
        public string? DescripcionFiscal { get; set; }
        public string? RazonSocial { get; set; }
        public string? RFC { get; set; }
        public string? TipoPersona { get; set; }
        public bool Activo { get; set; }
    }
}
