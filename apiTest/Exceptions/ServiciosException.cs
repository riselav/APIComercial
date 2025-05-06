namespace Voalaft.API.Exceptions
{
    public class ServiciosException : Exception
    {
        public ServiciosException() { }
        public ServiciosException(string message) : base(message) { }
        public ServiciosException(string message, Exception innerException) : base(message, innerException)
        { }
        public string Metodo { get; set; }
        public string ErrorMessage { get; set; }
    }
}
