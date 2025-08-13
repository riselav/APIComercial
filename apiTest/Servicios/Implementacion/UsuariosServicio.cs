using System.Globalization;
using System.Text;
using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Entidades.Menu;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class UsuariosServicio : IUsuariosServicio
    {
        private readonly IUsuariosRepositorio _usuariosRepositorio;
        private readonly ILogger<UsuariosServicio> _logger;

        public UsuariosServicio(ILogger<UsuariosServicio> logger, IUsuariosRepositorio usuariosRepositorio)
        {
            _logger = logger;
            _usuariosRepositorio = usuariosRepositorio;
        }

        public async Task<List<Usuarios>> Lista()
        {
            try
            {
                return await _usuariosRepositorio.Lista();
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario)
        {
            try
            {
                return await _usuariosRepositorio.ObtenerMenuUsuario(nUsuario);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener los usuario")
                {
                    Metodo = "ObtenerMenuUsuario",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Usuarios> ObtenerPorUsuario(string usuario)
        {
            try
            {
                return await _usuariosRepositorio.ObtenerPorUsuario(usuario);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener los usuario")
                {
                    Metodo = "ObtenerPorUsuario",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Usuarios> AccesoUsuario(Usuarios usuario)
        {
            try
            {
                return await _usuariosRepositorio.AccesoUsuario(usuario);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener el usuario")
                {
                    Metodo = "AccesoUsuario",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<Usuarios> AccesoEmpleado(Usuarios Empleado)
        {
            try
            {
                return await _usuariosRepositorio.AccesoEmpleado(Empleado);
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener el usuario")
                {
                    Metodo = "AccesoEmpleado",
                    ErrorMessage = ex.Message,
                };
            }
        }
        
            public async Task<Dictionary<string, object>> get_menu_web_usuario(int nUsuario)
        //public async Task<MenuNavigationRoute> get_menu_web_usuario(int nUsuario)
        {
            try
            {
                MenuNavigationRoute result = new();
                List<MenuDataRow> menuData = await _usuariosRepositorio.get_menu_web_usuario(nUsuario);
                if (menuData == null || !menuData.Any())
                {
                    throw new ArgumentException("No se encontraron menús para el usuario proporcionado.");
                }
                var arbolMenu = _ConstruirArbolMenu(menuData);
                var menuNavegacion = _ConstruirMenuNavegacion(arbolMenu);
                var rutas = _ConstruirRutasRecursivas(arbolMenu, new List<RouteItem>(), new List<string>());
                result.navigation = menuNavegacion;
                result.routes = rutas;
                // 3. Devolver el resultado final
                return new Dictionary<string, object>
                {
                    { "navigation", menuNavegacion },
                    { "routes", rutas }
                };
               // return result;
            }
            catch (DataAccessException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) No se pudo obtener el menu")
                {
                    Metodo = "get_menu_web_usuario",
                    ErrorMessage = ex.Message,
                };

            }

        }
        private static Dictionary<short, NavigationItem> _ConstruirArbolMenu(List<MenuDataRow> menuItems)
        {
            var nodos = new Dictionary<short, NavigationItem>();

            // Primer paso: Crear todos los nodos
            foreach (var menu in menuItems)
            {
                // Asegurarse de que el padre exista si no ha sido procesado
                if (menu.MenuIdParent.HasValue && !nodos.ContainsKey(menu.MenuIdParent.Value))
                {
                    nodos[menu.MenuIdParent.Value] = new NavigationItem
                    {
                        id = menu.MenuIdParent.Value,
                        title = menu.MenuDescripcionParent,
                        icon = menu.MenuIconoParent ?? "",
                        padreId = null,
                        menuUrl = null,
                        children = new List<NavigationItem>()
                    };
                }

                // Crear el nodo actual
                if (!nodos.ContainsKey(menu.MenuId))
                {
                    nodos[menu.MenuId] = new NavigationItem
                    {
                        id = menu.MenuId,
                        title = menu.MenuDescripcion,
                        icon = menu.MenuIcono ?? "",
                        padreId = menu.MenuIdParent,
                        menuUrl = menu.MenuUrl,
                        children = new List<NavigationItem>()
                    };
                }
            }

            // Segundo paso: Enlazar hijos a padres
            var arbol = new Dictionary<short, NavigationItem>();
            foreach (var nodo in nodos.Values)
            {
                if (nodo.padreId.HasValue && nodos.ContainsKey(nodo.padreId.Value))
                {
                    nodos[nodo.padreId.Value].children.Add(nodo);
                }
                else
                {
                    arbol.Add(nodo.id, nodo);
                }
            }

            return arbol;
        }

        private static List<NavigationItem> _ConstruirMenuNavegacion(Dictionary<short, NavigationItem> nodosPrincipales)
        {
            var menu = new List<NavigationItem>();
            foreach (var nodo in nodosPrincipales.Values) // O por un campo de orden
            {
                var elementoMenu = new NavigationItem
                {
                    id = nodo.id,
                    title = nodo.title,
                    icon = nodo.icon,
                    segment = RemoverAcentos(nodo.title).ToLower().Replace(" ", "-"),
                    children = nodo.children.Any() ? _ConstruirSubmenus(nodo.children) : null
                };
                menu.Add(elementoMenu);
            }
            return menu;
        }

        private static List<NavigationItem> _ConstruirSubmenus(List<NavigationItem> hijos)
        {
            var submenus = new List<NavigationItem>();
            foreach (var hijo in hijos)
            {
                var submenu = new NavigationItem
                {
                    id = hijo.id,
                    title = hijo.title,
                    icon = hijo.icon,
                    segment = RemoverAcentos(hijo.title).ToLower().Replace(" ", "-"),
                    children = hijo.children.Any() ? _ConstruirSubmenus(hijo.children) : null
                };
                submenus.Add(submenu);
            }
            return submenus;
        }

        private static List<RouteItem> _ConstruirRutasRecursivas(Dictionary<short, NavigationItem> nodos, List<RouteItem> rutas, List<string> padres)
        {
            foreach (var nodo in nodos.Values)
            {
                var pathSegment = RemoverAcentos(nodo.title).ToLower().Replace(" ", "-");
                var currentPath = new List<string>(padres) { pathSegment };

                if (nodo.children != null && nodo.children.Any())
                {
                    // Si tiene hijos, convierte la lista de hijos a diccionario y sigue recursivamente
                    var hijosDict = nodo.children.ToDictionary(hijo => hijo.id);
                    _ConstruirRutasRecursivas(hijosDict, rutas, currentPath);
                }
                else if (!string.IsNullOrEmpty(nodo.menuUrl))
                {
                    // Es un nodo final con URL, lo agregamos a las rutas
                    rutas.Add(new RouteItem
                    {
                        id = nodo.id.ToString(),
                        path = string.Join("/", currentPath),
                        component = nodo.menuUrl
                    });
                }
            }
            return rutas;
        }

        public static string RemoverAcentos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return texto;

            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        // --- FUNCIÓN SIMULADA ---
        // Reemplaza esto con tu llamada real a la base de datos
        private static List<MenuDataRow> _obtenerMenuUsuario(string userId /*, DbContext db */)
        {
            // Aquí iría tu código para llamar al SP:
            // var results = db.Database.SqlQuery<MenuDataRow>($"EXEC portal_precios.get_menu_usuario {userId}").ToList();
            // return results;

            // Datos de ejemplo para demostración:
            return new List<MenuDataRow>
            {
                new MenuDataRow { MenuId = 10, MenuIdParent = null, MenuDescripcionParent = "Catálogos", MenuDescripcion = "Catálogos", MenuOrden = 1 },
                new MenuDataRow { MenuId = 11, MenuIdParent = 10, MenuDescripcionParent = "Catálogos", MenuDescripcion = "Productos", MenuOrden = 1, MenuUrl = "catalogos/productos", MenuIcono = "fa-box" },
                new MenuDataRow { MenuId = 12, MenuIdParent = 10, MenuDescripcionParent = "Catálogos", MenuDescripcion = "Clientes", MenuOrden = 2, MenuUrl = "catalogos/clientes", MenuIcono = "fa-users" },
                new MenuDataRow { MenuId = 20, MenuIdParent = null, MenuDescripcionParent = "Reportes", MenuDescripcion = "Reportes", MenuOrden = 2 },
                new MenuDataRow { MenuId = 21, MenuIdParent = 20, MenuDescripcionParent = "Reportes", MenuDescripcion = "Ventas", MenuOrden = 1, MenuUrl = "reportes/ventas", MenuIcono = "fa-chart-line" }
            };
        }
    }
}