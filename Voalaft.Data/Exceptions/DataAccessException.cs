
namespace Voalaft.Data.Exceptions
{
    public class DataAccessException : Exception
    {
        public DataAccessException() { }
        public DataAccessException(string message) : base(message) { }
        public DataAccessException(string message, Exception innerException) : base(message, innerException)
        { }
        public string Metodo { get; set; }
        public string ProcedureName { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }
    
}
