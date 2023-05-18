using FluentPos.Identity.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FluentPos.Identity.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        const string IdentitySchemaName = "Identity";
        builder.ToTable("Users", IdentitySchemaName);
    }
}
