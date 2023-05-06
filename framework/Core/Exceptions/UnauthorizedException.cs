using System.Net;

namespace FSH.Framework.Core.Exceptions;

public class UnauthorizedException : CustomException
{
    public string Error { get; set; }
    public string Description { get; set; }
    public UnauthorizedException(string error = default!, string description = default!) : base(error, HttpStatusCode.Unauthorized)
    {
        Error = error;
        Description = description;
    }
}
