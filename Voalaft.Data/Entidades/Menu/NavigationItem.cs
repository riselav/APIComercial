using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Menu
{
    public class NavigationItem
    {
        public short Id { get; set; }
        public string Segment { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public List<NavigationItem> Children { get; set; }

        // Propiedades adicionales para la construcción del árbol
        public string MenuUrl { get; set; }
        public short? PadreId { get; set; }
    }
}
