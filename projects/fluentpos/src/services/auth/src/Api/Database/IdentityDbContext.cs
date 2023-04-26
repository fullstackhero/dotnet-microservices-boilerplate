using Microsoft.EntityFrameworkCore;

namespace FluentPOS.Auth.Api.Database;

public class IdentityDbContext : DbContext
{
    public IdentityDbContext(DbContextOptions options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) { }
}
