using FSH.Framework.Core.Exceptions;
using System.Net;

namespace FluentPos.Identity.Application.Users.Exceptions;
public class UserRegistrationException : CustomException
{
    public UserRegistrationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message, statusCode)
    {
    }
}
