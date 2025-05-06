using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatArticuloVenta
    {
        public int Articulo { get; set; }

        public string? Clave { get; set; }

        public string? Descripcion { get; set; }

        public decimal PrecioUnitarioNeto { get; set; }

        public int idSucursal { get; set; }

        public int idListaPrecio { get; set; }

        public decimal PrecioUnitarioSinImpuestos { get; set; }

        public decimal ImpuestoIVAUnitario { get; set; }

        public int IdImpuestoIVA { get; set; }

        public decimal PorcentajeImpuestoIVA { get; set; }

        public decimal? ImpuestoIEPSUnitario { get; set; }

        public int? IdImpuestoIEPS { get; set; }

        public decimal? PorcentajeImpuestoIEPS { get; set; }

        public bool esIEPSImporte { get; set; }

        public decimal CostoUnitario { get; set; }
      

        /// <summary>
        /// Para cuando venga de una promoción activa.
        /// </summary>
        public decimal ImporteDescuentoUnitario { get; set; }

        /// <summary>
        /// Para cuando venga de una promoción activa.
        /// </summary>
        public decimal? PorcentajeDescuentoUnitario { get; set; }

        public decimal? Existencia { get; set; }

        public bool Activo { get; set; }
    }
}
