using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FluentPOS.Lite.IDS.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}
