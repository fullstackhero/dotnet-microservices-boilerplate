using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FSH.Core.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message) : base(message, null, HttpStatusCode.NotFound)
    {
    }
}