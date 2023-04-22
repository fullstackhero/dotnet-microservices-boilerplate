using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSH.Microservices.Core.Domain;
public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    public string? CreatedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; } = DateTime.UtcNow;
    public string? LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();
    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
    public void AddDomainEvent(DomainEvent @event)
    {
        DomainEvents.Add(@event);
    }
}
