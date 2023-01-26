using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSH.Core.Domain;

public abstract class EventBase : IDomainEvent
{
    public string EventType { get { return GetType().FullName!; } }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public string CorrelationId { get; init; }
    public IDictionary<string, object> MetaData { get; } = new Dictionary<string, object>();
    public abstract void Flatten();
}