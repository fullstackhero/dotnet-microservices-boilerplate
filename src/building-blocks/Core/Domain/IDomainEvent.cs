using MediatR;

namespace FSH.Core.Domain;

public interface IDomainEvent : INotification
{
    DateTime CreatedAt { get; }
    IDictionary<string, object> MetaData { get; }
}
