using Dapr.Client;
using FSH.Core.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Infrastructure.Bus.Dapr;

public class DaprEventBus : IEventBus
{
    private readonly DaprClient _daprClient;
    private readonly DaprEventBusOptions _options;
    private readonly ILogger<DaprEventBus> _logger;

    public DaprEventBus(DaprClient daprClient, IOptions<DaprEventBusOptions> options, ILogger<DaprEventBus> logger)
    {
        _daprClient = daprClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string[] topics = default, CancellationToken token = default) where TEvent : IDomainEvent
    {
        var pubsubName = _options.PubSubName ?? "pubsub";
        if (topics is not null)
        {
            foreach (string topic in topics)
            {
                _logger.LogInformation("Publishing event {@Event} to {PubsubName}.{TopicName}", @event, pubsubName, topic);
                await _daprClient.PublishEventAsync(pubsubName, topic, @event, token);
            }
        }
        else
        {
            var topic = @event.GetType().Name;
            _logger.LogInformation("Publishing event {@Event} to {PubsubName}.{TopicName}", @event, pubsubName, topic);
            await _daprClient.PublishEventAsync(pubsubName, topic, @event, token);
        }
    }

    public Task SubscribeAsync<TEvent>(string[] topics = null, CancellationToken token = default) where TEvent : IDomainEvent
    {
        throw new NotImplementedException();
    }
}
