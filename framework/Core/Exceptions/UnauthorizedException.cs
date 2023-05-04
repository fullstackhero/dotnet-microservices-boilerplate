using System.Net;

namespace FSH.Microservices.Core.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException() : base("Unauthorized Request.", HttpStatusCode.Unauthorized)
    {
    }
}
