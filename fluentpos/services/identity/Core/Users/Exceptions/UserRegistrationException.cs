using FSH.Framework.Core.Exceptions;
using System.Net;

namespace FluentPOS.Identity.Core.Users.Exceptions;
public class UserRegistrationException : CustomException
{
    public UserRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
    {
    }
}
