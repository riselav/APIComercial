
namespace Voalaft.Data.Entidades
{
    public class CatProductosBase
    {
        public int ProductoBase { get; set; }
        public string? Descripcion { get; set; }
        public short TipoUnidad { get; set; }
        public string? DescripcionTipoUnidad { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
