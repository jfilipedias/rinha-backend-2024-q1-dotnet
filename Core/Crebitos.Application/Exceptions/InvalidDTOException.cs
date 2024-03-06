namespace Crebitos.Application;

public class InvalidDTOException : Exception
{
    public InvalidDTOException() { }
    public InvalidDTOException(string message) : base(message) { }
    public InvalidDTOException(string message, Exception innerException) : base(message, innerException) { }
}
