using FluentPos.Catalog.Domain.Products;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FluentPos.Catalog.Application.Consumers;
public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventConsumer> _logger;

    public ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        _logger.LogInformation("Message is {message}", context.Message);
        return Task.CompletedTask;
    }
}
