namespace Application.Common.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("User does not have required permissions") { }
    
    public ForbiddenAccessException(string message) : base(message) { }
    
    public ForbiddenAccessException(string message, Exception innerException) 
        : base(message, innerException) { }
}
