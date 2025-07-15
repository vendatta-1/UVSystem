using UVS.Common.Domain;

namespace UVS.Common.Application.Exceptions;

public class UVSException:Exception
{
    public UVSException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}