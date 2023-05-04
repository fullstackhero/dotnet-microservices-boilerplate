namespace FSH.Microservices.Core.Events;
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, string[] topics = default!, CancellationToken token = default) where TEvent : IEvent;
}
