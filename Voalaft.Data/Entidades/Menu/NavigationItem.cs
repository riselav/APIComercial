using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Menu
{
    public class NavigationItem
    {
        public short id { get; set; }
        public string segment { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public List<NavigationItem> children { get; set; }

        // Propiedades adicionales para la construcción del árbol
        public string menuUrl { get; set; }
        public short? padreId { get; set; }
    }
}
