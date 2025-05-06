
namespace Voalaft.Data.Entidades
{
    public class CatArticulos
    {
        public int Articulo { get; set; }
        public string? Clave { get; set; }
        public string? Descripcion { get; set; }
        public int Unidad { get; set; }
        public string? UnidadDescripcion { get; set; }
        public string? Presentacion { get; set; }
        public int Linea { get; set; }
        public string? LineaDescripcion { get; set; }
        public int Sublinea { get; set; }
        public string? sublineaDescripcion { get; set; }
        public int Marca { get; set; }
        public string? MarcaDescripcion { get; set; }
        public string? ClaveSAT { get; set; }
        public bool bLote { get; set; }
        public bool bSerie { get; set; }
        public bool bPedimento { get; set; }
        public bool bInsumoFinal { get; set; }
        public bool bProductoBase { get; set; }
        public int IdProductoBase { get; set; }
        public string? ProductoBaseDescripcion { get; set; }
        public int IdUnidadRelacional { get; set; }
        public decimal Equivalencia { get; set; }
        public bool bManejaInventario { get; set; }
        public int TipoArticulo { get; set; }
        public decimal PrecioGeneral { get; set; }
        public int IdImpuestoIVA { get; set; }
        public int IdImpuestoIEPS { get; set; }
        public decimal ImporteImpuestoIEPS { get; set; }
        public bool bDesglosaImpuestoIEPS { get; set; }
        public int IdListaPrecio { get; set; }
        public int IdMoneda { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
        public List<SucursalesCombo>? PreciosList { get; set; }
        public short? TipoUnidad {  get; set; }

    }
}
