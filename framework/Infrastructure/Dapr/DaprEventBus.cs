using Dapr.Client;
using FSH.Framework.Core.Events;
using Microsoft.Extensions.Logging;

namespace FSH.Framework.Infrastructure.Dapr;
internal class DaprEventBus : IEventBus
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprEventBus> _logger;

    public DaprEventBus(DaprClient daprClient, ILogger<DaprEventBus> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string pubSubName = default!, CancellationToken token = default) where TEvent : IEvent
    {
        if (string.IsNullOrEmpty(pubSubName)) pubSubName = DaprConstants.RMQPubSub;
        string topicName = @event.GetType().Name;
        _logger.LogInformation("Publishing event {@Event} to {PubsubName}.{TopicName}", @event, pubSubName, topicName);
        try
        {
            await _daprClient.PublishEventAsync(pubSubName, topicName, (object)@event, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
        }
    }
}
