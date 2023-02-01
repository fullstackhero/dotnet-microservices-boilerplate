using FSH.Core.Common;
using FSH.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace FSH.Persistence.EfCore;

public abstract class DbContextBase : DbContext
{
    protected readonly IAuthenticatedUser _authenticatedUser;
    protected DbContextBase(DbContextOptions options, IAuthenticatedUser authenticatedUser) : base(options)
    {
        _authenticatedUser = authenticatedUser;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var authenticatedUserId = _authenticatedUser.Id;
        if (authenticatedUserId is not null)
        {
            HandleAuditingBeforeSaveChanges(authenticatedUserId);
        }

        int result = await base.SaveChangesAsync(cancellationToken);

        //await SendDomainEventsAsync();

        return result;
    }

    private void HandleAuditingBeforeSaveChanges(string authenticatedUserId)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = authenticatedUserId;
                    entry.Entity.LastModifiedBy = authenticatedUserId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = authenticatedUserId;
                    break;

                case EntityState.Deleted:
                    entry.Entity.DeletedBy = authenticatedUserId;
                    entry.Entity.DeletedOn = DateTime.UtcNow;
                    entry.State = EntityState.Modified;

                    break;
            }
        }
    }

}
