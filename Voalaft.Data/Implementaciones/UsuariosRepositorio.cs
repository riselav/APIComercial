
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Voalaft.Data.DB;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;
using Voalaft.Utilerias;

using SQLConnector; // Asegúrate de que el namespace sea el correcto

namespace Voalaft.Data.Implementaciones
{
    public class UsuariosRepositorio : IUsuariosRepositorio
    {
        private readonly Conexion _conexion;
        private readonly ILogger<UsuariosRepositorio> _logger;

        public UsuariosRepositorio(ILogger<UsuariosRepositorio> logger,Conexion conexion)
        {
            _conexion = conexion;
            _logger = logger;
        }

        public async Task<List<Usuarios>> Lista()
        {
            List<Usuarios> usuarios = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT nFolio,cUsuario,cPassword,bAdministrador,nEmpleado,bActivo" +
                        ",cNombre,cApellidoPaterno,cApellidoMaterno FROM CAT_Usuarios (NOLOCK)", con);
                    cmd.CommandType = CommandType.Text;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(
                                new Usuarios()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nFolio"])
                                    ,Usuario = ConvertUtils.ToString(reader["cUsuario"])
                                    ,Password = ConvertUtils.ToString(reader["cPassword"])
                                    ,Administrador = ConvertUtils.ToBoolean(reader["bAdministrador"])
                                    ,Empleado = ConvertUtils.ToInt32(reader["nEmpleado"])
                                    ,Activo = ConvertUtils.ToBoolean(reader["bActivo"])
                                    ,Nombre = ConvertUtils.ToString(reader["cNombre"])
                                    ,ApellidoPaterno = ConvertUtils.ToString(reader["cApellidoPaterno"])
                                    ,ApellidoMaterno = ConvertUtils.ToString(reader["cApellidoMaterno"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo= "Lista",
                    ErrorMessage= ex.Message,
                    ErrorCode = 1
                };
            }
            
            return usuarios;
        }

        public async Task<List<MenuUsuario>> ObtenerMenuUsuario(int nUsuario)
        {
            List<MenuUsuario> usuarios = [];
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("select distinct cUsuario,mm.nModulo,mm.cDescripcion as moduloDescripcion,om.nOpcion, om.cDescripcion as opcionDescripcion "
                    + " from CAT_Usuarios u (nolock) "
                    + " inner join CAT_PerfilesUsuarios pu (nolock) on u.nFolio = pu.nUsuario "
                    + " inner join CAT_Perfiles(nolock)per on per.nPerfil = pu.nPerfil "
                    + " inner join CAT_PermisosMenu (nolock)pm on pm.nPerfil = pu.nPerfil "
                    + " inner join CAT_OpcionesMenu (nolock)om on om.nOpcion = pm.nOpcion "
                    + " inner join CAT_ModulosMenu (nolock)mm on mm.nModulo = om.nModulo "
                    + " where u.nFolio = @Usuario and u.bActivo = 1 and pu.bActivo = 1 and per.bActivo = 1 and pm.bActivo = 1 and om.bActivo = 1 and mm.bActivo = 1 "
                    + " order by u.cUsuario, mm.nModulo, om.nOpcion ", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Usuario", SqlDbType.Int).Value = nUsuario;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(
                                new MenuUsuario()
                                {   
                                    Usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    Modulo = ConvertUtils.ToInt32(reader["nModulo"]),
                                    ModuloDescripcion = ConvertUtils.ToString(reader["moduloDescripcion"]),
                                    Opcion = ConvertUtils.ToInt32(reader["nOpcion"]),
                                    OpcionDescripcion = ConvertUtils.ToString(reader["opcionDescripcion"]),
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener los usuario")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }

            return usuarios;
        }

        public async Task<Usuarios> ObtenerPorUsuario(string usuario)
        {
            Usuarios usuarios = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT nFolio,cUsuario,cPassword,bAdministrador,nEmpleado,bActivo" +
                        ",cNombre,cApellidoPaterno,cApellidoMaterno FROM CAT_Usuarios (NOLOCK) where cUsuario=@Usuario", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = usuario;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios =
                                new Usuarios()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nFolio"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    Password = ConvertUtils.ToString(reader["cPassword"]),
                                    Administrador = ConvertUtils.ToBoolean(reader["bAdministrador"]),
                                    Empleado = ConvertUtils.ToInt32(reader["nEmpleado"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
                                    ApellidoPaterno = ConvertUtils.ToString(reader["cApellidoPaterno"]),
                                    ApellidoMaterno = ConvertUtils.ToString(reader["cApellidoMaterno"])
                                };
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener el usuario")
                {
                    Metodo = "ObtenerPorUsuario",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return usuarios;
        }

        public async Task<Usuarios> AccesoUsuario(Usuarios usuario)
        {
            Usuarios usuarioValido = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT nFolio,cUsuario,cPassword,bAdministrador,nEmpleado,bActivo" +
                        ",cNombre,cApellidoPaterno,cApellidoMaterno FROM CAT_Usuarios (NOLOCK) where cUsuario=@Usuario", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = usuario.Usuario;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarioValido =
                                new Usuarios()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nFolio"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    Password = ConvertUtils.ToString(reader["cPassword"]),
                                    Administrador = ConvertUtils.ToBoolean(reader["bAdministrador"]),
                                    Empleado = ConvertUtils.ToInt32(reader["nEmpleado"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
                                    ApellidoPaterno = ConvertUtils.ToString(reader["cApellidoPaterno"]),
                                    ApellidoMaterno = ConvertUtils.ToString(reader["cApellidoMaterno"])
                                };
                            break;
                        }
                    }

                    if (usuarioValido != null){
                        // Instancia de la clase de criptografía
                        CriptografiaDAL criptografia = new CriptografiaDAL();

                        // Texto encriptado previamente con el mismo algoritmo y claves
                        string textoEncriptado = usuarioValido.Password; 

                        // Usar el método Desencriptar
                        string textoDesencriptado = criptografia.DesencriptarContraseña (textoEncriptado);
                        string textoDesencriptadoSha1 = criptografia.ObtenerSHA256(textoDesencriptado);

                        bool coincide = textoDesencriptadoSha1.Equals(usuario.Password, StringComparison.OrdinalIgnoreCase);
                        if (!coincide)
                        {
                            usuarioValido = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener el usuario")
                {
                    Metodo = "AccesoUsuario",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return usuarioValido;
        }

        public async Task<Usuarios> AccesoEmpleado(Usuarios Empleado)
        {
            Usuarios usuarioValido = null;
            try
            {
                using (var con = _conexion.ObtenerSqlConexion())
                {
                    con.Open();
                    var cmd = new SqlCommand("SELECT nFolio,cUsuario,cPassword,bAdministrador,nEmpleado,bActivo" +
                        ",cNombre,cApellidoPaterno,cApellidoMaterno FROM CAT_Usuarios (NOLOCK) where nEmpleado=@Empleado", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@Empleado", SqlDbType.Int).Value = Empleado.Empleado;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarioValido =
                                new Usuarios()
                                {
                                    Folio = ConvertUtils.ToInt32(reader["nFolio"]),
                                    Usuario = ConvertUtils.ToString(reader["cUsuario"]),
                                    Password = ConvertUtils.ToString(reader["cPassword"]),
                                    Administrador = ConvertUtils.ToBoolean(reader["bAdministrador"]),
                                    Empleado = ConvertUtils.ToInt32(reader["nEmpleado"]),
                                    Activo = ConvertUtils.ToBoolean(reader["bActivo"]),
                                    Nombre = ConvertUtils.ToString(reader["cNombre"]),
                                    ApellidoPaterno = ConvertUtils.ToString(reader["cApellidoPaterno"]),
                                    ApellidoMaterno = ConvertUtils.ToString(reader["cApellidoMaterno"])
                                };
                            break;
                        }
                    }

                    if (usuarioValido != null)
                    {
                        // Instancia de la clase de criptografía
                        CriptografiaDAL criptografia = new CriptografiaDAL();

                        // Texto encriptado previamente con el mismo algoritmo y claves
                        string textoEncriptado = usuarioValido.Password;

                        // Usar el método Desencriptar
                        string textoDesencriptado = criptografia.DesencriptarContraseña(textoEncriptado);
                        string textoDesencriptadoSha1 = criptografia.ObtenerSHA256(textoDesencriptado);

                        bool coincide = textoDesencriptadoSha1.Equals(Empleado.Password, StringComparison.OrdinalIgnoreCase);
                        if (!coincide)
                        {
                            usuarioValido = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string className = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[0] : "";
                string methodName = ex.StackTrace != null ? ex.StackTrace.Split('\n')[0].Trim().Split(' ')[1] : "";
                int lineNumber = ex.StackTrace == null ? 1 : int.Parse(ex.StackTrace.Split('\n')[0].Trim().Split(':')[1]);

                _logger.LogError($"Error en {className}.{methodName} (línea {lineNumber}): {ex.Message}");
                throw new DataAccessException("Error(rp) No se pudo obtener el usuario")
                {
                    Metodo = "AccesoEmpleado",
                    ErrorMessage = ex.Message,
                    ErrorCode = 1
                };
            }
            return usuarioValido;
        }
    }
}