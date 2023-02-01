using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSH.Infrastructure.Authentication;

public class AuthOptions
{
    public string Authority { get; set; }
    public string Audience { get; set; }
}
