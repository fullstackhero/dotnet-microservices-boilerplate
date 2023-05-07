using FSH.Framework.Core.Events;
using MassTransit;
using System.Text.Json.Serialization;

namespace FSH.Framework.Core.Domain;
public abstract class BaseEntity : BaseEntity<DefaultIdType>
{
    protected BaseEntity() => Id = NewId.Next().ToGuid();
}

public abstract class BaseEntity<TId> : IBaseEntity<TId>
{
    [JsonPropertyOrder(-1)]
    public TId Id { get; protected set; } = default!;
    public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CreatedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; } = DateTime.UtcNow;
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LastModifiedBy { get; private set; }
    [JsonIgnore]
    public bool IsDeleted { get; private set; }
    [JsonIgnore]
    private readonly List<IDomainEvent> _domainEvents = new();
    [JsonIgnore]
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
    public void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        var dequeuedEvents = _domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeuedEvents;
    }
}
