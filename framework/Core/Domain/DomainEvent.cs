using FSH.Framework.Core.Events;

namespace FSH.Framework.Core.Domain;
public abstract class DomainEvent : IDomainEvent
{
    public Guid Id { get; }
    public DateTime CreationDate { get; }

    public IDictionary<string, object> MetaData { get; }

    public DomainEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        MetaData = new Dictionary<string, object>();
    }
}
