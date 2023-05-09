namespace FSH.Framework.Core.Events;

public class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreationDate { get; }

    public IDictionary<string, object> MetaData { get; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
        MetaData = new Dictionary<string, object>();
    }
}
