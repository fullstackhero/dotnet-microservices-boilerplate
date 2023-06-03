using FSH.Framework.Core.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FSH.Framework.Infrastructure.Messaging;

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publisher;
    private readonly ILogger<EventPublisher> _logger;

    public EventPublisher(IPublishEndpoint publisher, ILogger<EventPublisher> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken token = default) where TEvent : IEvent
    {
        return _publisher.Publish(@event, token);
    }
}
