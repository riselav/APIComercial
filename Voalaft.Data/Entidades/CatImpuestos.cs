
namespace Voalaft.Data.Entidades
{
    public class CatImpuestos
    {
        public int Impuesto { get; set; }
        public string? Descripcion { get; set; }
        public decimal Porcentaje { get; set; }
        public short Tipo { get; set; }
        public bool ImpuestoImporte { get; set; }
        public bool Excento { get; set; }
        public string? c_Impuesto { get; set; }
        public string? DescripcionCImpuesto { get; set; }
        public string? TipoFactor { get; set; }
        public decimal TasaOCuota { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
