using System.Net;

namespace FSH.Core.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, HttpStatusCode.NotFound)
    {
    }
}
