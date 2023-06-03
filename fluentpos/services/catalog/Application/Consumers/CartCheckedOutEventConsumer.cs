using FluentPos.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FluentPos.Catalog.Application.Consumers;
public class CartCheckedOutEventConsumer : IConsumer<CartCheckedOutEvent>
{
    private readonly ILogger<CartCheckedOutEventConsumer> _logger;

    public CartCheckedOutEventConsumer(ILogger<CartCheckedOutEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CartCheckedOutEvent> context)
    {
        _logger.LogInformation("CC Numbers is {ccNo}", context.Message.CreditCardNumber);
        return Task.CompletedTask;
    }
}
