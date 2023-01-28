using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FSH.Core.Exceptions;

public class CustomException : Exception
{
    public List<string> Messages { get; }

    public HttpStatusCode StatusCode { get; }

    public CustomException(string message, List<string> errors = default, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        : base(message)
    {
        Messages = errors;
        StatusCode = statusCode;
    }
}
