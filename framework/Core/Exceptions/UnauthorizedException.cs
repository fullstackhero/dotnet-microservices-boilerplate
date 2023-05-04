using System.Net;

namespace FSH.Framework.Core.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException() : base("Unauthorized Request.", HttpStatusCode.Unauthorized)
    {
    }
}
