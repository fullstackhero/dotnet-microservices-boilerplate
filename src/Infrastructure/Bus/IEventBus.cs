using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FSH.Core.Domain;

namespace FSH.Infrastructure.Bus;

public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event, string[] topics = default!, CancellationToken token = default)
        where TEvent : IDomainEvent;

    Task SubscribeAsync<TEvent>(string[] topics = default!, CancellationToken token = default)
        where TEvent : IDomainEvent;
}
