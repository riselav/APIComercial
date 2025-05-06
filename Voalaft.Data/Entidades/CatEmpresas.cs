
namespace Voalaft.Data.Entidades
{
    public class CatEmpresas
    {
        public int Empresa { get; set; }
        public string? Descripcion { get; set; }
        public int GrupoEmpresarial { get; set; }
        public string? DescripcionGrupoEmpresarial { get; set; }
        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
