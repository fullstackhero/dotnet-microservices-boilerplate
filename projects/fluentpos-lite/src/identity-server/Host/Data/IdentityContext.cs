using FluentPOS.Lite.IDS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FluentPOS.Lite.IDS.Data;

public sealed class IdentityContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid,
    IdentityUserClaim<Guid>,
    IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public IdentityContext(
        DbContextOptions<IdentityContext> options,
        IHttpContextAccessor httpContextAccessor) :
        base(options)
    {
        if (httpContextAccessor == null)
            throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
