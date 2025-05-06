using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatEmpleadosServicio: ICatEmpleadosServicio  
    {
        private readonly ICatEmpleadosRepositorio  _EmpleadoRepositorio;
        private readonly ILogger<CatEmpleadosServicio > _logger;

        public CatEmpleadosServicio (ILogger<CatEmpleadosServicio > logger, ICatEmpleadosRepositorio  EmpleadoRepositorio)
        {
            _logger = logger;
            _EmpleadoRepositorio = EmpleadoRepositorio;
        }

        public async Task<CatEmpleados> IME_CAT_Empleados(CatEmpleados catEmpleado)
        {
            try
            {
                return await _EmpleadoRepositorio.IME_CAT_Empleados (catEmpleado);
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al guardar el empleado ")
                {
                    Metodo = "IME_CAT_Empleados",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatEmpleados >> Lista()
        {
            try
            {
                return await _EmpleadoRepositorio.Lista();
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al obtener los empleados")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatEmpleados> ObtenerEmpleado(int nEmpleado)
        {
            try
            {
                return await _EmpleadoRepositorio.ObtenerEmpleado (nEmpleado);
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

                _logger.LogError($"Error en {className}.{methodName} (CatEmpleadosServicio {lineNumber}): {ex.Message}");
                throw new ServiciosException("Error(srv) al obtener el empleado")
                {
                    Metodo = "ObtenerEmpleado",
                    ErrorMessage = ex.Message,
                };
            }
        }




    }
}
