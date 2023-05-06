namespace FSH.Framework.Core.Events;
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, string pubSubName = default!, CancellationToken token = default) where TEvent : IEvent;
}
