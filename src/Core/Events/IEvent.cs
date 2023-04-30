using MediatR;

namespace FSH.Microservices.Core.Events;
public interface IEvent : INotification
{
    DefaultIdType Id { get; }
    DateTime CreationDate { get; }
    IDictionary<string, object> MetaData { get; }
}
