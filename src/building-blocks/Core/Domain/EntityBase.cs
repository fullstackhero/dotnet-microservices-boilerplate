using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSH.Core.Domain;

public abstract class EntityBase
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public DateTime Created { get; protected init; } = DateTime.UtcNow;
    public DateTime? Updated { get; protected set; }
}

public abstract class EntityRootBase : EntityBase, IAggregateRoot
{
    private readonly List<EventBase> _domainEvents = new();
    public IReadOnlyList<EventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(EventBase eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(EventBase eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }
}