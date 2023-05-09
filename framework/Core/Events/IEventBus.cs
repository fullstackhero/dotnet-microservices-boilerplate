namespace FSH.Framework.Core.Events;
public interface IEventBus
{
    Task PublishDomainEventAsync<TEvent>(TEvent @event, string pubSubName = default!, CancellationToken token = default) where TEvent : IDomainEvent;

    Task PublishIntegrationEventAsync<TEvent>(TEvent @event, string pubSubName = default!, CancellationToken token = default) where TEvent : IIntegrationEvent;
}
