
namespace Voalaft.Data.Entidades
{
    public class CatUnidadesRelacionales
    {
        public int UnidadRelacional { get; set; }
        public string? Descripcion { get; set; }
        public short TipoUnidad { get; set; }
        public string? DescripcionTipoUnidad { get; set; }
        public bool EsBase{ get; set; }
        public decimal Equivalencia { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
