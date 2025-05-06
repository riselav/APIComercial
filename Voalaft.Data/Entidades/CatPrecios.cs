
namespace Voalaft.Data.Entidades
{
    public class CatPrecios
    {
        public int? IdSucursal { get; set; }
        public int IdArticulo { get; set; }
        public decimal Precio { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
        public int? IdListaPrecio { get; set; }
        public int? IdMoneda { get; set; }
    }
}
