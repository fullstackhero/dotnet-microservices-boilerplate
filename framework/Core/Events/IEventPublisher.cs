namespace FSH.Framework.Core.Events;
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent;
}
