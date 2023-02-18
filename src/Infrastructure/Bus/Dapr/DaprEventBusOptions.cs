using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FSH.Infrastructure.Bus.Dapr;

public class DaprEventBusOptions
{
    public static string Name => "DaprEventBus";
    public string PubSubName { get; set; } = "pubsub";
}
