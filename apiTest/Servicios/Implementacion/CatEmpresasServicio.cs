using Voalaft.API.Exceptions;
using Voalaft.API.Servicios.Interfaces;
using Voalaft.Data.Entidades;
using Voalaft.Data.Exceptions;
using Voalaft.Data.Interfaces;

namespace Voalaft.API.Servicios.Implementacion
{
    public class CatEmpresasServicio : ICatEmpresasServicio
    {
        private readonly ICatEmpresasRepositorio _EmpresasRepositorio;
        private readonly ILogger<CatEmpresasServicio> _logger;

        public CatEmpresasServicio(ILogger<CatEmpresasServicio> logger, ICatEmpresasRepositorio EmpresasRepositorio)
        {
            _logger = logger;
            _EmpresasRepositorio = EmpresasRepositorio;
        }

        public async Task<CatEmpresas> IME_CatEmpresas(CatEmpresas catEmpresa)
        {
            try
            {
                return await _EmpresasRepositorio.IME_CatEmpresas(catEmpresa);
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
                throw new ServiciosException("Error(srv) No se pudo guardar la empresa")
                {
                    Metodo = "IME_CatEmpresas",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CatEmpresas>> Lista()
        {
            try
            {
                return await _EmpresasRepositorio.Lista();
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
                throw new ServiciosException("Error(srv) No se pudo obtener las Empresas")
                {
                    Metodo = "Lista",
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<CatEmpresas> ObtenerPorEmpresa(int nEmpresa)
        {
            try
            {
                return await _EmpresasRepositorio.ObtenerPorEmpresa(nEmpresa);
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
                throw new ServiciosException("Error(srv) No se pudo obtener las Empresas")
                {
                    Metodo = "ObtenerPorEmpresa",
                    ErrorMessage = ex.Message,
                };
            }
        }
    }
}
