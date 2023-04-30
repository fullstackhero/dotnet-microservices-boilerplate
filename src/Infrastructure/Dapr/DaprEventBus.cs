using Dapr.Client;
using FSH.Microservices.Core.Events;
using Microsoft.Extensions.Logging;

namespace FSH.Microservices.Infrastructure.Dapr;
internal class DaprEventBus : IEventBus
{
    private readonly DaprClient _daprClient;
    private readonly ILogger<DaprEventBus> _logger;

    public DaprEventBus(DaprClient daprClient, ILogger<DaprEventBus> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, string[] topics = default!, CancellationToken token = default) where TEvent : IEvent
    {
        //Assume single topic
        string topicName = @event.GetType().Name;

        _logger.LogInformation("Publishing event {@Event} to {PubsubName}.{TopicName}", @event, DaprConstants.PubSubName, topicName);
        try
        {
            await _daprClient.PublishEventAsync(DaprConstants.PubSubName, topicName, (object)@event, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace);
        }
    }
}
