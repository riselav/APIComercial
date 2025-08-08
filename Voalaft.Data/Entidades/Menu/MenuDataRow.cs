
namespace Voalaft.Data.Entidades.Menu
{
    public class MenuDataRow
    {
        public short MenuId { get; set; }
        public short? MenuIdParent { get; set; }
        public string MenuDescripcionParent { get; set; }
        public short? MenuOrdenParent { get; set; }
        public string MenuDescripcion { get; set; }
        public short? MenuOrden { get; set; }
        public string MenuUrl { get; set; }
        public string MenuIcono { get; set; }
    }

}
