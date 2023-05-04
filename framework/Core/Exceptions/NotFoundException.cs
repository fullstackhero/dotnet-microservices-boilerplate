using System.Net;

namespace FSH.Microservices.Core.Exceptions;
public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}
