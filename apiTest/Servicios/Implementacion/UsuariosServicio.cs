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

        public async Task<MenuNavigationRoute> get_menu_web_usuario(int nUsuario)
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
                //    // 3. Devolver el resultado final
                //    return new Dictionary<string, object>
                //{
                //    { "navigation", menuNavegacion },
                //    { "routes", rutas }
                //};
                return result;
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
                        Id = menu.MenuIdParent.Value,
                        Title = menu.MenuDescripcionParent,
                        Icon = "", // El ícono del padre usualmente se define en su propia fila
                        PadreId = null,
                        MenuUrl = null,
                        Children = new List<NavigationItem>()
                    };
                }

                // Crear el nodo actual
                if (!nodos.ContainsKey(menu.MenuId))
                {
                    nodos[menu.MenuId] = new NavigationItem
                    {
                        Id = menu.MenuId,
                        Title = menu.MenuDescripcion,
                        Icon = menu.MenuIcono ?? "",
                        PadreId = menu.MenuIdParent,
                        MenuUrl = menu.MenuUrl,
                        Children = new List<NavigationItem>()
                    };
                }
            }

            // Segundo paso: Enlazar hijos a padres
            var arbol = new Dictionary<short, NavigationItem>();
            foreach (var nodo in nodos.Values)
            {
                if (nodo.PadreId.HasValue && nodos.ContainsKey(nodo.PadreId.Value))
                {
                    nodos[nodo.PadreId.Value].Children.Add(nodo);
                }
                else
                {
                    arbol.Add(nodo.Id, nodo);
                }
            }

            return arbol;
        }

        private static List<NavigationItem> _ConstruirMenuNavegacion(Dictionary<short, NavigationItem> nodosPrincipales)
        {
            var menu = new List<NavigationItem>();
            foreach (var nodo in nodosPrincipales.Values.OrderBy(n => n.Title)) // O por un campo de orden
            {
                var elementoMenu = new NavigationItem
                {
                    Id = nodo.Id,
                    Title = nodo.Title,
                    Icon = nodo.Icon,
                    Segment = RemoverAcentos(nodo.Title).ToLower().Replace(" ", "-"),
                    Children = nodo.Children.Any() ? _ConstruirSubmenus(nodo.Children) : null
                };
                menu.Add(elementoMenu);
            }
            return menu;
        }

        private static List<NavigationItem> _ConstruirSubmenus(List<NavigationItem> hijos)
        {
            var submenus = new List<NavigationItem>();
            foreach (var hijo in hijos.OrderBy(h => h.Title))
            {
                var submenu = new NavigationItem
                {
                    Id = hijo.Id,
                    Title = hijo.Title,
                    Icon = hijo.Icon,
                    Segment = RemoverAcentos(hijo.Title).ToLower().Replace(" ", "-"),
                    Children = hijo.Children.Any() ? _ConstruirSubmenus(hijo.Children) : null
                };
                submenus.Add(submenu);
            }
            return submenus;
        }

        private static List<RouteItem> _ConstruirRutasRecursivas(Dictionary<short, NavigationItem> nodos, List<RouteItem> rutas, List<string> padres)
        {
            foreach (var nodo in nodos.Values)
            {
                var pathSegment = RemoverAcentos(nodo.Title).ToLower().Replace(" ", "-");
                var currentPath = new List<string>(padres) { pathSegment };

                if (nodo.Children != null && nodo.Children.Any())
                {
                    // Si tiene hijos, convierte la lista de hijos a diccionario y sigue recursivamente
                    var hijosDict = nodo.Children.ToDictionary(hijo => hijo.Id);
                    _ConstruirRutasRecursivas(hijosDict, rutas, currentPath);
                }
                else if (!string.IsNullOrEmpty(nodo.MenuUrl))
                {
                    // Es un nodo final con URL, lo agregamos a las rutas
                    rutas.Add(new RouteItem
                    {
                        Id = nodo.Id.ToString(),
                        Path = string.Join("/", currentPath),
                        Component = nodo.MenuUrl
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