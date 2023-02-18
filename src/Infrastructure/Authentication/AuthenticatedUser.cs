using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FSH.Core.Common;
using Microsoft.AspNetCore.Http;

namespace FSH.Infrastructure.Authentication;

public class AuthenticatedUser : IAuthenticatedUser
{
    public AuthenticatedUser(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is not null)
        {
            Id = user.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Claims = user.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
            Email = user.FindFirstValue(ClaimTypes.Email);
        }
    }

    public string Id { get; }
    public string Email { get; }
    public List<KeyValuePair<string, string>> Claims { get; set; }
}
